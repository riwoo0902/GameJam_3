using System;
using DevLib.AnimatorSystem;
using JJM.Scripts.Players.Stats;
using JJM.Scripts.PowerUp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JJM.Scripts.UI
{
    [RequireComponent(typeof(Animator))]
    public class PowerUpPanel : MonoBehaviour
    {
        [SerializeField] private AbstractPowerUpSo powerUpSo;

        [SerializeField] private Image powerUpIcon;
        [SerializeField] private TextMeshProUGUI powerUpName;
        [SerializeField] private TextMeshProUGUI powerUpLore;

        [SerializeField] private string priceTextStart;
        [SerializeField] private string priceTextEnd;
        [SerializeField] private TextMeshProUGUI powerUpPrice;

        [SerializeField] private HashDataSO hashIdle;
        [SerializeField] private HashDataSO hashEnd;

        
        public AbstractPowerUpSo PowerUpSo
        {
            get => powerUpSo;

            set
            {
                powerUpSo = value;
                ResetPowerUpPanel();
            }
        }
        private bool _buy;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnValidate()
        {
            ChangeTexts();
        }

        public void ResetPowerUpPanel()
        {
            ChangeTexts();
            _animator.Play(hashIdle.HashValue, 0, 0);
            _buy = false;
        }

        private void ChangeTexts()
        {
            if (powerUpSo == null)
                return;

            powerUpIcon.sprite = powerUpSo.Icon;
            powerUpName.text = powerUpSo.Name;
            powerUpLore.text = FormatLore(powerUpSo.Lore);
            powerUpPrice.text = $"{priceTextStart} {powerUpSo.Price}{priceTextEnd}";
        }

        private static string FormatLore(string lore)
        {
            if (string.IsNullOrWhiteSpace(lore))
                return string.Empty;

            string[] loreLines = lore.Split(
                new[] { ',' },
                StringSplitOptions.RemoveEmptyEntries
            );

            for (int i = 0; i < loreLines.Length; i++)
            {
                loreLines[i] = loreLines[i].Trim();
            }

            return string.Join("\n", loreLines);
        }

        public void PowerUpBuy()
        {
            if (_buy) return;
            
            int price = powerUpSo.Price;

            var module = PlayerStatManager.Instance.PlayerHealthModule;
            
            if (module.CurrentHealth > price)
            {
                module.TakeDamage(price);
                _buy = true;
                _animator.Play(hashEnd.HashValue, 0, 0);
                powerUpSo.PowerUpPlay();
            }
            else if (module.CurrentHealth > price)
            {
                //실패 이벤트
            }
            else
            {
                //샀을 때 체력이 0일 때 이벤트
            }
        }
    }
}