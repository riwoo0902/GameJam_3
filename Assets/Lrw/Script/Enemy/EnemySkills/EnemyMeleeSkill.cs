using Lrw.Script.CombatSystem;
using UnityEngine;

namespace Lrw.Script.Enemy.EnemySkills
{
    public class EnemyMeleeSkill : AbstractSkillModule
    {
        private float _coolTime;
        [SerializeField] private DamageCaster damageCaster;
        
        private void Update()
        {
            _coolTime -= Time.deltaTime;
        }

        public override bool CanUse(Transform target = null)
        {
            return _coolTime <= 0f;
        }

        public override void Use(Transform target = null)
        {
            _coolTime = 1;
            damageCaster.Cast(Stat.Value);
        }
        
    }
}