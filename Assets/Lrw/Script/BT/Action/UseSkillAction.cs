using System;
using Lrw.Script.Enemy;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Lrw.Script.BT.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "UseSkill", story: "[Enemy] UseSkill to [Target]", category: "Action", id: "6b1374ff87389117a3c8d3ab18549ab5")]
    public partial class UseSkillAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        protected override Status OnStart()
        {
            if (Enemy.Value == null || Target.Value == null) return Status.Failure;

            Enemy.Value.SkillModule.Use(Target.Value);
        
            return Status.Success;
        }
    
    }
}

