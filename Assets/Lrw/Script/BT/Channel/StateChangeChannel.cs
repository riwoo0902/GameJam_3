using System;
using Lrw.Script.Enemy;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

#if UNITY_EDITOR
namespace Lrw.Script.BT.Channel
{
    [CreateAssetMenu(menuName = "Behavior/Event Channels/StateChangeChannel")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "StateChangeChannel", message: "[State]", category: "Events", id: "60b3dc3ae711d6106893edd78a605535")]
    public sealed partial class StateChangeChannel : EventChannel<EnemyState> { }
}

