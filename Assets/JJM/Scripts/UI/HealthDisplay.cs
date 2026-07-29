using System;
using System.Globalization;
using JJM.Scripts.Players.Stats;
using TMPro;
using UnityEngine;

namespace JJM.Scripts.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class HealthDisplay : MonoBehaviour
    {
        private TextMeshProUGUI _textMesh;
        
        private void Awake()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            _textMesh.text = PlayerStatManager.Instance.PlayerHealthModule.CurrentHealth.ToString(CultureInfo.InvariantCulture);
        }
    }
}