using System;
using UnityEngine;

namespace Lrw.Script.Enemy.EnemySkills
{
    public class EnemyMeleeSkill : AbstractSkillModule
    {
        private float _coolTime;

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
            Debug.Log("Skill Use");
        }
        
    }
}