using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Lrw.Script.UI.SoundUI
{
    public enum AudioMixerGroupType
    {
        Master,
        BGM,
        SFX
    }

    public class SoundChangeBar : MonoBehaviour
    {
        private const float MinDecibel = -80f;
        private const float MinLinearVolume = 0.0001f;

        [SerializeField] private Scrollbar scrollbar;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioMixerGroupType type;

        private string ParameterName => type.ToString();

        private void OnEnable()
        {
            if (scrollbar == null || audioMixer == null)
            {
                Debug.LogError(
                    $"{nameof(SoundChangeBar)}: Scrollbar 또는 AudioMixer가 할당되지 않았습니다.",
                    this
                );

                return;
            }
            InitializeScrollbar();
            scrollbar.onValueChanged.AddListener(ChangedSlider);
        }

        private void OnDisable()
        {
            if (scrollbar != null)
            {
                scrollbar.onValueChanged.RemoveListener(ChangedSlider);
            }
        }

        private void InitializeScrollbar()
        {
            if (!audioMixer.GetFloat(ParameterName, out float decibel))
            {
                Debug.LogError(
                    $"AudioMixer에 노출된 파라미터 '{ParameterName}'을 찾을 수 없습니다.",
                    this
                );

                return;
            }

            float normalizedVolume = DecibelToNormalized(decibel);
            scrollbar.SetValueWithoutNotify(normalizedVolume);
        }

        private void ChangedSlider(float normalizedVolume)
        {
            float decibel = NormalizedToDecibel(normalizedVolume);

            if (!audioMixer.SetFloat(ParameterName, decibel))
            {
                Debug.LogError(
                    $"AudioMixer 파라미터 '{ParameterName}' 설정에 실패했습니다.",
                    this
                );
            }
        }

        private static float NormalizedToDecibel(float normalizedVolume)
        {
            if (normalizedVolume <= 0f)
            {
                return MinDecibel;
            }

            float clampedVolume = Mathf.Max(
                normalizedVolume,
                MinLinearVolume
            );

            return Mathf.Log10(clampedVolume) * 20f;
        }

        private static float DecibelToNormalized(float decibel)
        {
            if (decibel <= MinDecibel)
            {
                return 0f;
            }

            return Mathf.Pow(10f, decibel / 20f);
        }
    }
}