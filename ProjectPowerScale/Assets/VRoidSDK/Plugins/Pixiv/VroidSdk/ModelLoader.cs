using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Pixiv.VroidSdk.Api;
using Pixiv.VroidSdk.Api.DataModel;
using Pixiv.VroidSdk.Cache;
using Pixiv.VroidSdk.Cache.DataModel;
using Pixiv.VroidSdk.Cache.Migrate;
using Pixiv.VroidSdk.IO;
using Pixiv.VroidSdk.Unity.Crypt;
using UniGLTF;
using UnityEngine;
using UniVRM10;

namespace Pixiv.VroidSdk
{
    /// <summary>
    /// モデルの読み込みユーティリティ
    ///
    /// 原則、コールバックは非同期に呼び出される
    ///
    /// Asyncがつくものは非同期に実行されるかつ、成功結果の<see cref="Task"/>が返ってくる
    /// </summary>
    public static class ModelLoader
    {
        private static ModelDataCache s_cache;
        private static string s_temporaryCachePath;
        private static SynchronizationContext s_context;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="config">VRoid SDKの利用設定</param>
        /// <param name="downloadApi">ダウンロードAPIをリクエストするクライアント</param>
        /// <param name="cacheEncryptPassword">モデルデータを暗号化するアプリケーション固有の任意文字列</param>
        /// <param name="maxCacheCount">保持するモデルの最大数</param>
        /// <param name="context">メインスレッドの<see cref="SynchronizationContext"/></param>
        public static void Initialize(ISdkConfig config, IDownloadLicensePublishable downloadApi, string cacheEncryptPassword, uint maxCacheCount = 10, SynchronizationContext context = null)
        {
            s_temporaryCachePath = Application.temporaryCachePath;
            s_context = context ?? SynchronizationContext.Current;

            var cryptoFileReadWrite = new CharacterModelVersionCacheFile(CharacterModelVersionCacheFilePath(config), new UnityFileCryptor(cacheEncryptPassword));
            var storage = DownloadLicenseCacheStorage.Load(DownloadLicenseCacheStorageFilePath(config), maxCacheCount);
            s_cache = new ModelDataCache(downloadApi, cryptoFileReadWrite, storage, new LegacyCacheMigrator(config.SymbolPrefix, Application.persistentDataPath));
        }

        /// <summary>
        /// VRMモデルを読み込む
        /// </summary>
        /// <remarks>
        /// デバイスにキャッシュがある場合はキャッシュから読み込む。
        /// キャッシュが存在しない場合はダウンロードを行う。
        /// </remarks>
        /// <param name="characterModel">ダウンロードをするモデル</param>
        /// <param name="onSuccess">成功コールバック</param>
        /// <param name="onProgress">進捗コールバック</param>
        /// <param name="onFailed">失敗コールバック</param>
        /// <param name="materialGenerator">VRMロード時に使用するMaterialDescriptorGenerator</param>
        /// <exception cref="Exception"><see cref="ModelLoader"/>が初期化されていない場合にスローされる例外</exception>
        /// <seealso cref="LoadVrmAsync"/>
        public static void LoadVrm(CharacterModel characterModel, Action<GameObject> onSuccess, Action<float> onProgress,
            Action<ModelLoadFailException> onFailed, IMaterialDescriptorGenerator materialGenerator = null)
        {
            if (s_cache == null)
            {
                throw new Exception("ModelLoader is not initialized. Please call ModelLoader.Initialize");
            }

            s_cache.Fetch(characterModel,
                (binary) => LoadVRMFromBinary(binary, onSuccess, onFailed, materialGenerator),
                onProgress,
                (error) => onFailed?.Invoke(new ModelLoadFailException(error)));
        }

        /// <summary>
        /// VRMモデルを読み込む
        /// </summary>
        /// <remarks>
        /// デバイスにキャッシュがある場合はキャッシュから読み込む。
        /// キャッシュが存在しない場合はダウンロードを行う。
        /// </remarks>
        /// <param name="characterModel">ダウンロードをするモデル</param>
        /// <param name="onProgress">進捗コールバック</param>
        /// <exception cref="Exception"><see cref="ModelLoader"/>が初期化されていない場合にスローされる例外</exception>
        /// <seealso cref="LoadVrm"/>
        public static async Task<GameObject> LoadVrmAsync(CharacterModel characterModel, Action<float> onProgress, IMaterialDescriptorGenerator materialGenerator = null)
        {
            if (s_cache == null)
            {
                throw new Exception("ModelLoader is not initialized. Please call ModelLoader.Initialize");
            }

            try
            {
                var binary = await s_cache.FetchAsync(characterModel, onProgress);
                return await LoadVRMFromBinaryAsync(binary, materialGenerator);
            }
            catch (Exception e)
            {
                throw new ModelLoadFailException(e.Message);
            }
        }

        private static void LoadVRMFromBinary(byte[] characterBinary, Action<GameObject> onVrmModelLoaded, Action<ModelLoadFailException> onFailed, IMaterialDescriptorGenerator materialGenerator = null)
        {
            Vrm10.LoadBytesAsync(characterBinary, canLoadVrm0X: true, showMeshes: true, materialGenerator: materialGenerator).ContinueWith(result =>
            {
                s_context.Post(_ =>
                {
                    if (result.Exception != null)
                    {
                        var exception = result.Exception.Flatten().InnerException;
                        onFailed?.Invoke(new ModelLoadFailException(exception.Message));
                    }
                    else
                    {
                        onVrmModelLoaded?.Invoke(result.Result.gameObject);
                    }
                }, null);
            });
        }

        private static Task<GameObject> LoadVRMFromBinaryAsync(byte[] characterBinary, IMaterialDescriptorGenerator materialGenerator = null)
        {
            var taskResult = new TaskCompletionSource<GameObject>();

            Vrm10.LoadBytesAsync(characterBinary, canLoadVrm0X: true, showMeshes: true, materialGenerator: materialGenerator).ContinueWith(result =>
            {
                s_context.Post(_ =>
                {
                    if (result.Exception != null)
                    {
                        var exception = result.Exception.Flatten().InnerException;
                        taskResult.SetException(exception);
                    }
                    else
                    {
                        taskResult.SetResult(result.Result.gameObject);
                    }
                }, null);
            });

            return taskResult.Task;
        }

        private static string CharacterModelVersionCacheFilePath(ISdkConfig config)
        {
            Debug.Assert(!string.IsNullOrEmpty(s_temporaryCachePath), "s_temporaryCachePath is null or empty.");

            return Path.Combine(s_temporaryCachePath,
                                            config.BaseDirectoryName,
                                            "character_model_version_cache_files");
        }

        private static string DownloadLicenseCacheStorageFilePath(ISdkConfig config)
        {
            Debug.Assert(!string.IsNullOrEmpty(s_temporaryCachePath), "s_temporaryCachePath is null or empty.");

            return Path.Combine(s_temporaryCachePath,
                                        config.BaseDirectoryName,
                                        $"{config.SymbolPrefix}_download_license_cache");
        }
    }
}
