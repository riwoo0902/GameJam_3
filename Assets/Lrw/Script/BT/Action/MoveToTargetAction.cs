using System;
using Lrw.Script.Enemy;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Lrw.Script.BT.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "MoveToTarget", story: "[Enemy] move to [Target]", category: "Enemy", id: "2fe60f798015a16907982ca73ffbff1e")]
    public partial class MoveToTargetAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<Enemy.Enemy> Enemy;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        protected override Status OnStart()
        {
            if (Enemy.Value == null || Target.Value == null || Enemy.Value.MoveModule == null) return Status.Failure;

            Enemy.Value.MoveModule.SetDestination(Target.Value.transform.position);
            
            return Status.Success;
        }
        
    }
}

