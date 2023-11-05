using System.Threading.Tasks;
using UnityEngine;
using UniVRM10;

namespace VRoidSDK.Examples.ArtworkExample.View
{
    public class Cubeman : MonoBehaviour
    {
        private async Task<Vrm10Instance> LoadVrm(byte[] vrmData)
        {
            return await Vrm10.LoadBytesAsync(vrmData, canLoadVrm0X: true, showMeshes: true);
        }

        private async Task ShowVrm(byte[] vrmData)
        {
            var instance = await LoadVrm(vrmData);
            instance.transform.SetParent(transform, false);
        }

        private void OnEnable()
        {
            var textAsset = Resources.Load<TextAsset>("Cubeman.vrm");
            var vrmData = textAsset.bytes;
            Resources.UnloadAsset(textAsset);
            var _ = ShowVrm(vrmData);
        }
    }
}
