using System;
using UnityEngine;

namespace Lrw.Script.CombatSystem
{
    public class CircleCaster : DamageCaster
    {
        [SerializeField] private LayerMask targetMash;
        [SerializeField] private float radius;
        
        protected override Collider2D[] GetColliders()
        {
            return Physics2D.OverlapCircleAll(transform.position, radius,targetMash);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}