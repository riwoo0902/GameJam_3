using System;
using Lrw.Script.Agent.HealthSystem;
using Publics.Scripts;
using UnityEngine;

namespace JJM.Scripts.Players.Stats
{
    public class PlayerStatManager : MonoSingleton<PlayerStatManager>
    {
        [field: SerializeField] public float SPD { get; set; } = 1.0f;
        [field: SerializeField] public float ATK { get; set; } = 1.0f;
        [field: SerializeField] public float ATS { get; set; } = 1.0f;
        [field: SerializeField] public float PSPD { get; set; } = 1.0f;
        [field: SerializeField] public float VIS { get; set; } = 1.0f;
        [field: SerializeField] public HealthModule PlayerHealthModule { get; set; }
    }
}