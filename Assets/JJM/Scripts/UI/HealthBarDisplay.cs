using System.Collections.Generic;
using JJM.Scripts.Players.Stats;
using UnityEngine;

namespace JJM.Scripts.UI
{
    public class HealthBarDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject bar;

        private readonly List<GameObject> _createdBars = new();
        private bool _isSubscribed;

        private void Start()
        {
            if (bar == null)
            {
                Debug.LogError($"{nameof(HealthBarDisplay)}에 Bar 프리팹이 할당되지 않았습니다.");
                return;
            }

            if (PlayerStatManager.Instance == null ||
                PlayerStatManager.Instance.PlayerHealthModule == null)
            {
                Debug.LogError("PlayerHealthModule을 찾을 수 없습니다.");
                return;
            }

            PlayerStatManager.Instance.PlayerHealthModule.OnHealthChanged
                += HandleBarUpdate;

            _isSubscribed = true;

            UpdateBarCount(
                PlayerStatManager.Instance.PlayerHealthModule.CurrentHealth
            );
        }

        private void OnDestroy()
        {
            if (!_isSubscribed)
                return;

            if (PlayerStatManager.Instance == null ||
                PlayerStatManager.Instance.PlayerHealthModule == null)
                return;

            PlayerStatManager.Instance.PlayerHealthModule.OnHealthChanged
                -= HandleBarUpdate;
        }

        private void HandleBarUpdate(
            float currentHealth,
            float previousHealth,
            float maxHealth)
        {
            UpdateBarCount(currentHealth);
        }

        private void UpdateBarCount(float currentHealth)
        {
            int targetCount = Mathf.Max(
                0,
                Mathf.CeilToInt(currentHealth)
            );

            while (_createdBars.Count < targetCount)
            {
                GameObject createdBar = Instantiate(
                    bar,
                    transform,
                    false
                );

                createdBar.SetActive(true);
                _createdBars.Add(createdBar);
            }

            while (_createdBars.Count > targetCount)
            {
                int lastIndex = _createdBars.Count - 1;
                GameObject barToRemove = _createdBars[lastIndex];

                _createdBars.RemoveAt(lastIndex);

                barToRemove.SetActive(false);
                Destroy(barToRemove);
            }
        }
    }
}