using System;
using Lrw.Script.Agent.HealthSystem;
using UnityEngine;

namespace JJM
{
    public class BossHealthBarRe : MonoBehaviour
    {
        private void Start()
        {
            BossHealthBar.Instance.Health = GetComponentInChildren<HealthModule>();
        }
    }
}