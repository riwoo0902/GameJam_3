using DevLib.ModuleSystem;
using Lrw.Script.Agent.HealthSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JJM.Scripts.Players
{
    public class Player : ModuleOwner
    {
        [SerializeField] private HealthModule healthModule;
        
        private void Update()
        {
            if (Keyboard.current.hKey.wasPressedThisFrame)
            {
                healthModule.TakeDamage(5);
            }
        }
    }
}
