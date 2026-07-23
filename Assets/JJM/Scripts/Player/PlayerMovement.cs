using DevLib.AnimatorSystem;
using DevLib.ModuleSystem;
using Publics.Agent;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JJM.Scripts.Player
{
    public class PlayerMovement : Module
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D rb;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float deceleration = 8f;

        [Header("Movement")] 
        [SerializeField] private HashDataSO inputXHash;
        [SerializeField] private HashDataSO inputYHash;
        
        private Vector2 _moveDir;
        private IRenderer _renderer;
        
        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _renderer = _owner.GetModule<IRenderer>();    
        }

        private void FixedUpdate()
        {
            Vector2 inputDirection = Vector2.ClampMagnitude(_moveDir, 1f);
            Vector2 targetVelocity = inputDirection * moveSpeed;

            bool isMoving = inputDirection.sqrMagnitude > 0.001f;
            float velocityChangeSpeed = isMoving
                ? acceleration
                : deceleration;

            rb.linearVelocity = Vector2.MoveTowards(
                rb.linearVelocity,
                targetVelocity,
                velocityChangeSpeed * Time.fixedDeltaTime
            );
            
            if (_moveDir.magnitude < 0.1f) return;
            Animator ownerAnimator = _renderer.Animator;
            ownerAnimator.SetFloat(inputXHash.HashValue, _moveDir.x);
            ownerAnimator.SetFloat(inputYHash.HashValue, _moveDir.y);
        }

        public void OnMove(InputValue value)
        {
            _moveDir = value.Get<Vector2>();
        }
    }
}