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

        [Header("Hash")]
        [SerializeField] private HashDataSO idleHash;
        [SerializeField] private HashDataSO moveHash;
        
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

            Animator ownerAnimator = _renderer.Animator;
            ownerAnimator.Play(_moveDir.magnitude > 0.1f ? moveHash.HashValue : idleHash.HashValue, 0);
        }

        public void OnMove(InputValue value)
        {
            _moveDir = value.Get<Vector2>();
        }
    }
}