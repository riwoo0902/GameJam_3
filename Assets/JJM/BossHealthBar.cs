using Lrw.Script.Agent.HealthSystem;
using Publics.Scripts;
using UnityEngine;

namespace JJM
{
    public class BossHealthBar : MonoSingleton<BossHealthBar>
    {
        [SerializeField] private Transform bossHealthBar;
        [SerializeField, Min(1f)] private float maxHealth = 1500f;
        [field: SerializeField] public HealthModule Health { get; set; }

        private Vector3 _initialScale;

        protected override void Awake()
        {
            base.Awake();
            _initialScale = bossHealthBar.localScale;
        }

        private void Update()
        {
            float healthRatio = Mathf.Clamp01(
                Health.CurrentHealth / maxHealth
            );

            bossHealthBar.localScale = new Vector3(
                _initialScale.x * healthRatio,
                _initialScale.y,
                _initialScale.z
            );
        }
    }
}