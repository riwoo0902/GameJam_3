using DevLib.AnimatorSystem;
using Publics.Scripts;
using UnityEngine;

namespace JJM.Scripts.UI
{
    [RequireComponent(typeof(Animator))]
    public class CanvasManager : MonoSingleton<CanvasManager>
    {
        [SerializeField] private HashDataSO powerUpStart;
        [SerializeField] private HashDataSO powerUpEnd;

        private Animator _animator;
        
        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        public void PowerUpPlay()
        {
            _animator.Play(powerUpStart.HashValue, 0, 0);
        }

        public void PowerUpEnd()
        {
            _animator.Play(powerUpEnd.HashValue, 0, 0);
        }
    }
}