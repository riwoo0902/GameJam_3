using System;
using System.Globalization;
using System.Reflection;
using JJM.Scripts.Players.Stats;
using TMPro;
using UnityEngine;

namespace JJM.Scripts.UI
{
    public class PlayerStatText : MonoBehaviour
    {
        private const BindingFlags PropertyFlags =
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic;

        [SerializeField] private TMP_Text valueText;
        [SerializeField] private string propertyName;

        private PropertyInfo _propertyInfo;

        private void Start()
        {
            CacheProperty();
            Refresh();
        }

        private void Update()
        {
            Refresh();
        }

        private void CacheProperty()
        {
            PlayerStatManager statManager = PlayerStatManager.Instance;

            if (statManager == null)
            {
                Debug.LogError(
                    "PlayerStatManager.Instance가 null입니다.",
                    this
                );

                return;
            }

            _propertyInfo = statManager
                .GetType()
                .GetProperty(propertyName, PropertyFlags);

            if (_propertyInfo == null)
            {
                Debug.LogError(
                    $"PlayerStatManager에 '{propertyName}' 프로퍼티가 없습니다.",
                    this
                );

                return;
            }

            if (!_propertyInfo.CanRead)
            {
                Debug.LogError(
                    $"'{propertyName}' 프로퍼티를 읽을 수 없습니다.",
                    this
                );

                _propertyInfo = null;
            }
        }

        public void Refresh()
        {
            if (valueText == null ||
                _propertyInfo == null ||
                PlayerStatManager.Instance == null)
            {
                return;
            }

            object value = _propertyInfo.GetValue(
                PlayerStatManager.Instance
            );

            string formattedValue = value is IFormattable formattable
                ? formattable.ToString(
                    "0.0",
                    CultureInfo.InvariantCulture
                )
                : value?.ToString() ?? string.Empty;

            valueText.text =
                $"{propertyName} {formattedValue}F";
        }
    }
}