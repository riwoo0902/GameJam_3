using JJM.Scripts.Players;
using JJM.Scripts.Players.Stats;
using UnityEngine;

namespace JJM.Scripts.PowerUp
{
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

        protected Players.Player Player { get; private set; }

        public virtual void PowerUpPlay()
        {
            if (!TryInitializePlayer())
            {
                return;
            }

            if (PowerUpManager.Instance == null)
            {
                Debug.LogError("PowerUpManager.Instance가 존재하지 않습니다.");
                return;
            }

            PlayerStatManager statManager = PlayerStatManager.Instance;

            if (statManager == null)
            {
                Debug.LogError("PlayerStatManager.Instance가 존재하지 않습니다.");
                return;
            }

            PowerUpManager.Instance.AllAbstractPowerUps.Add(this);

            statManager.SPD += SPD;
            statManager.ATK += ATK;
            statManager.ATS += ATS;
            statManager.PSPD += PSPD;
            statManager.VIS += VIS;
        }

        protected bool TryInitializePlayer()
        {
            if (Player != null)
            {
                return true;
            }

            Player = FindFirstObjectByType<Players.Player>();

            if (Player != null)
            {
                return true;
            }

            Debug.LogError($"{name}: 활성화된 Player를 찾지 못했습니다.");
            return false;
        }

        protected virtual void OnDisable()
        {
            Player = null;
        }
    }
}