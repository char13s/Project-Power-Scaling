#if !UNITY_EDITOR && UNITY_ANDROID && ENABLE_MONO
#error Unsupported for Mono Build on Android. Please use IL2CPP Scripting Backend for Android Devices.
#endif

#if NET_2_0 || NET_2_0_SUBSET
#error .NET 3.5 runtime is not supported.
#endif
