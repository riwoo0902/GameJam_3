using DevLib.AnimatorSystem;
using DevLib.ModuleSystem;
using JJM.Scripts.Players.Stats;
using Lrw.Script.Agent.StatSystem;
using Publics.Agent;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JJM.Scripts.Players
{
    public class PlayerMovement : Module, IAfterInitModule
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D rb;

        [Header("Movement")]
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float deceleration = 8f;

        [Header("Hash")]
        [SerializeField] private HashDataSO idleHash;
        [SerializeField] private HashDataSO moveHash;
        
        [Header("Stat")] 
        [SerializeField] private StatDataSo moveSpeedDataSo;

        private Stat _speedStat;
        private float MoveSpeed => _speedStat.Value;        
        
        private Vector2 _moveDir;
        private IRenderer _renderer;
        
        private IStatModule _statModule;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _statModule = owner.GetModule<IStatModule>();
            _renderer = _owner.GetModule<IRenderer>();    
        }

        public void AfterInit()
        {
            _speedStat = _statModule.GetStat(moveSpeedDataSo);
            Debug.Assert(_speedStat != null, "StatModule is not found");    
        }
        private void FixedUpdate()
        {
            Vector2 inputDirection = Vector2.ClampMagnitude(_moveDir, 1f);
            Vector2 targetVelocity = inputDirection * (MoveSpeed * PlayerStatManager.Instance.SPD);

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