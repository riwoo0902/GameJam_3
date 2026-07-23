using System;
using Lrw.Script.Enemy;
using Unity.Behavior;
using UnityEngine;

namespace Lrw.Script.BT.Condition
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "CanUseSkill", story: "[Enemy] CanUse Skill to [Target]", category: "Conditions", id: "54a5d2bcbdaa77cdfe5b55dbef12a87f")]
    public partial class CanUseSkillCondition : Unity.Behavior.Condition
    {
        [SerializeReference] public BlackboardVariable<Enemy.Enemy> Enemy;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        public override bool IsTrue()
        {
            if (Enemy.Value == null || Target.Value == null)
            {
                Debug.LogWarning("CheckEnemy distance condition : 값이 할당되지 않아 항상 false");
                return false;
            }
        
            return Enemy.Value.SkillModule.CanUse(Target.Value);
        }
    
    }
}
