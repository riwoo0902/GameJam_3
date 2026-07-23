using Lrw.Script.Enemy;
using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/StateChangeChannel")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "StateChangeChannel", message: "[State]", category: "Events", id: "60b3dc3ae711d6106893edd78a605535")]
public sealed partial class StateChangeChannel : EventChannel<EnemyState> { }

