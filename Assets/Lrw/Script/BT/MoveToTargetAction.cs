using System;
using JJM.Scripts.Player;
using Lrw.Script.Enemy;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Lrw.Script.BT
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "MoveToTarget", story: "[Enemy] move to [Target]", category: "Enemy", id: "2fe60f798015a16907982ca73ffbff1e")]
    public partial class MoveToTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [SerializeReference] public BlackboardVariable<Player> Target;

        protected override Status OnStart()
        {
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            return Status.Success;
        }

        protected override void OnEnd()
        {
        }
    }
}

