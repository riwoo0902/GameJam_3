using DG.Tweening;
using UnityEngine;

namespace Publics.Scripts.Agent
{
    public class HitEvent : MonoBehaviour
    {
        private static readonly int AmountId =
            Shader.PropertyToID("_Amount");

        [SerializeField] private ParticleSystem hitEffect;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private float fadeDuration = 0.25f;
        [SerializeField] private Ease fadeEase = Ease.OutQuad;

        private Material _material;
        private Tween _fadeTween;

        private void Awake()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (spriteRenderer == null)
            {
                Debug.LogError(
                    $"{name}: SpriteRenderer가 없습니다.",
                    this
                );

                return;
            }

            _material = spriteRenderer.material;

            if (!_material.HasProperty(AmountId))
            {
                Debug.LogError(
                    $"{name}: 머티리얼에 _Amount 프로퍼티가 없습니다.",
                    this
                );

                return;
            }

            _material.SetFloat(AmountId, 0f);
        }

        public void EventPlay()
        {
            hitEffect?.Play();

            if (_material == null)
            {
                return;
            }

            _fadeTween?.Kill();

            _material.SetFloat(AmountId, 1f);

            _fadeTween = _material
                .DOFloat(0f, AmountId, fadeDuration)
                .SetEase(fadeEase);
        }

        private void OnDisable()
        {
            _fadeTween?.Kill();
            _fadeTween = null;

            if (_material != null)
            {
                _material.SetFloat(AmountId, 0f);
            }
        }
    }
}