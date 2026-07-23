using System.Collections.Generic;
using Publics.Scripts;
using UnityEngine;

namespace JJM.Scripts.PowerUp
{
    public class PowerUpManager : MonoSingleton<PowerUpManager>
    {
        [field: SerializeField] public List<AbstractPowerUpSo> AllAbstractPowerUps { get; set; }
    }
}