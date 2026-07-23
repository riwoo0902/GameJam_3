using UnityEngine;

namespace Lrw.Script.Enemy
{
    [DefaultExecutionOrder(-1)]
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Transform target;
        public static Transform Target { get; private set; }

        private void Awake()
        {
            Debug.Assert(target != null,"targetPlayer is null");

            Target = target;
        }
        
    }
}