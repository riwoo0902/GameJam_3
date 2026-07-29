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
    [RequireComponent(typeof(RectTransform))]
    public class PowerUpPanel : MonoBehaviour
    {
        [field: SerializeField]
        public AbstractPowerUpSo PowerUpSo { get; set; }

        [Header("UI")]
        [SerializeField] private Image powerUpIcon;
        [SerializeField] private TextMeshProUGUI powerUpName;
        [SerializeField] private TextMeshProUGUI powerUpLore;
        [SerializeField] private TextMeshProUGUI powerUpPrice;

        [Header("Price")]
        [SerializeField] private string priceTextStart;
        [SerializeField] private string priceTextEnd;

        [Header("Animation")]
        [SerializeField] private HashDataSO hashIdle;
        [SerializeField] private HashDataSO hashEnd;

        private Animator _animator;
        private RectTransform _rectTransform;
        private bool _buy;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnValidate()
        {
            ChangeTexts();
        }

        public void ResetPowerUpPanel()
        {
            _buy = false;

            SelectRandomPowerUp();
            ChangeTexts();

            if (!_animator.isActiveAndEnabled)
            {
                Debug.LogWarning($"{name}: Animator가 비활성화 상태입니다.");
                return;
            }

            // 기존 End 상태를 Idle 상태로 변경
            _animator.Play(hashIdle.HashValue, 0, 0f);
            _animator.Update(0f);

            // End 애니메이션에서 남은 Y = 0을 확실하게 초기화
            SetScaleY(1f);
        }

        private void SelectRandomPowerUp()
        {
            PowerUpManager manager = PowerUpManager.Instance;

            if (manager == null)
            {
                Debug.LogError($"{name}: PowerUpManager가 없습니다.");
                return;
            }

            var powerUpList = manager.AllAbstractPowerUpList;

            if (powerUpList == null || powerUpList.Count == 0)
            {
                Debug.LogError($"{name}: PowerUp 목록이 비어 있습니다.");
                return;
            }

            PowerUpSo = powerUpList[
                UnityEngine.Random.Range(0, powerUpList.Count)
            ];
        }

        private void SetScaleY(float y)
        {
            Vector3 scale = _rectTransform.localScale;
            scale.y = y;
            _rectTransform.localScale = scale;
        }

        private void ChangeTexts()
        {
            if (PowerUpSo == null)
            {
                return;
            }

            if (powerUpIcon != null)
            {
                powerUpIcon.sprite = PowerUpSo.Icon;
            }

            if (powerUpName != null)
            {
                powerUpName.text = PowerUpSo.Name;
            }

            if (powerUpLore != null)
            {
                powerUpLore.text = FormatLore(PowerUpSo.Lore);
            }

            if (powerUpPrice != null)
            {
                powerUpPrice.text =
                    $"{priceTextStart} {PowerUpSo.Price}{priceTextEnd}";
            }
        }

        private static string FormatLore(string lore)
        {
            if (string.IsNullOrWhiteSpace(lore))
            {
                return string.Empty;
            }

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
            if (_buy || PowerUpSo == null)
            {
                return;
            }

            PlayerStatManager statManager = PlayerStatManager.Instance;

            if (statManager == null ||
                statManager.PlayerHealthModule == null)
            {
                Debug.LogError($"{name}: 플레이어 체력 모듈이 없습니다.");
                return;
            }

            var healthModule = statManager.PlayerHealthModule;
            int price = PowerUpSo.Price;

            if (healthModule.CurrentHealth < price)
            {
                // 체력이 가격보다 부족한 경우
                return;
            }

            if (Mathf.Approximately(healthModule.CurrentHealth, price))
            {
                // 구매하면 체력이 정확히 0이 되는 경우
                return;
            }

            _buy = true;

            healthModule.TakeDamage(price);
            PowerUpSo.PowerUpPlay();

            // 선택된 패널만 Y Scale 1 → 0 애니메이션
            SetScaleY(1f);
            _animator.Play(hashEnd.HashValue, 0, 0f);
        }
    }
}