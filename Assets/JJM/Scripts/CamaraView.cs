using JJM.Scripts.Players.Stats;
using Unity.Cinemachine;
using UnityEngine;

namespace JJM.Scripts
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachineCamera;

        [SerializeField] private float viewMultiplier = 6f;
        [SerializeField] private float minViewSize = 3f;
        [SerializeField] private float maxViewSize = 30f;

        private void Update()
        {
            if (cinemachineCamera == null ||
                PlayerStatManager.Instance == null)
            {
                return;
            }

            float targetSize =
                PlayerStatManager.Instance.VIS * viewMultiplier;

            cinemachineCamera.Lens.OrthographicSize = Mathf.Clamp(
                targetSize,
                minViewSize,
                maxViewSize
            );
        }
    }
}