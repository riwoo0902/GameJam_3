using System.Collections;
using DevLib.ModuleSystem;
using Lrw.Script.CombatSystem;
using Lrw.Script.Enemy.MoveSystem;
using UnityEngine;

namespace Lrw.Script.Enemy.EnemySkills
{
    public class GGHSkill : AbstractSkillModule
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
        
        
        protected override void Use(Transform target = null)
        {
            _coolTime = 5f;
            
            
            
            
        }

        
    }
}