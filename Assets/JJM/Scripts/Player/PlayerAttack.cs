using System.Collections;
using DevLib.ModuleSystem;
using DevLib.ObjectPool.Runtime;
using JJM.Scripts.Players.Stats;
using Lrw.Script.Agent.StatSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace JJM.Scripts.Players
{
    public class PlayerAttack : Module, IAfterInitModule
    {
        [Header("Pool")]
        [SerializeField] private PoolItemSO projectile;
        [SerializeField] private PoolManagerSO poolManager;

        [Header("Stat")] 
        [SerializeField] private StatDataSo projectiveSpeedDataSo;
        [SerializeField] private StatDataSo projectiveDamageDataSo;
        [SerializeField] private StatDataSo attackSpeedDataSo;

        [Header("Unity Event")]
        public UnityEvent OnAttack;
        
        private Stat _speedStat;
        public float ProjectiveSpeed => _speedStat.Value;        
        private Stat _damageStat;
        public float ProjectiveDamage => _damageStat.Value;        
        private Stat _attackSpeedStat;
        public float AttackSpeedStat => _attackSpeedStat.Value;
        
        
        private bool _canFire = true;

        private IPlayerRotation _playerRotation;
        private IStatModule _statModule;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _statModule = owner.GetModule<IStatModule>();

            _playerRotation = owner.GetModule<PlayerRotation>();
        }

        public void AfterInit()
        {
            _speedStat = _statModule.GetStat(projectiveSpeedDataSo);
            Debug.Assert(_speedStat != null, "StatModule is not found");              
            _damageStat = _statModule.GetStat(projectiveDamageDataSo);
            Debug.Assert(_damageStat != null, "StatModule is not found");                
            _attackSpeedStat = _statModule.GetStat(attackSpeedDataSo);
            Debug.Assert(_attackSpeedStat != null, "StatModule is not found");    
        }
        private void Update()
        {
            if (Mouse.current == null)
            {
                return;
            }

            if (!Mouse.current.leftButton.isPressed)
            {
                return;
            }

            if (!_canFire)
            {
                return;
            }

            Vector2 direction = _playerRotation.MouseRelativePosition;

            if (direction.sqrMagnitude <= 0.001f)
            {
                return;
            }

            Fire(direction);
        }

        private void Fire(Vector2 direction)
        {
            _canFire = false;

            PlayerProjective projectileInstance =
                poolManager.Pop<PlayerProjective>(projectile);

            if (projectileInstance == null)
            {
                _canFire = true;
                return;
            }
            
            OnAttack?.Invoke();

            projectileInstance.Speed =  ProjectiveSpeed * PlayerStatManager.Instance.PSPD;
            projectileInstance.Damage = ProjectiveDamage * PlayerStatManager.Instance.ATK;

            float angle = GetFourDirectionAngle(direction);

            projectileInstance.transform.SetPositionAndRotation(
                _owner.transform.position,
                Quaternion.Euler(0f, 0f, angle)
            );

            StartCoroutine(FireCoolDown());
        }

        private IEnumerator FireCoolDown()
        {
            yield return new WaitForSeconds(AttackSpeedStat / PlayerStatManager.Instance.ATS);
            _canFire = true;
        }

        public static float GetFourDirectionAngle(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                return direction.x >= 0f ? 0f : 180f;
            }

            return direction.y >= 0f ? 90f : -90f;
        }

    }
}