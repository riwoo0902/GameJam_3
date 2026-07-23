using System;
using JJM.Scripts.Players;
using JJM.Scripts.Players.Stats;
using UnityEngine;

namespace JJM.Scripts.PowerUp
{
    [DefaultExecutionOrder(-1)]
    public abstract class AbstractPowerUpSo : ScriptableObject
    {
        [Header("Icon/Name/Lore/Price")]
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Lore { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        
        [Header("Stat")]
        [SerializeField] private float SPD;
        [SerializeField] private float ATK;
        [SerializeField] private float ATS;
        [SerializeField] private float PSPD;
        [SerializeField] private float VIS;

        public virtual void PowerUpPlay()
        {
            PowerUpManager.Instance.AllAbstractPowerUps.Add(this);
            var psm = PlayerStatManager.Instance;
            psm.SPD += SPD;
            psm.ATK += ATK;
            psm.ATS += ATS;
            psm.PSPD += PSPD;
            psm.VIS += VIS;
        }
    }
}