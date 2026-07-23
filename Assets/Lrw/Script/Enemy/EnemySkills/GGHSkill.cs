using DevLib.ObjectPool.Runtime;
using Lrw.Script.CombatSystem;
using Lrw.Script.Enemy.Bullet;
using UnityEngine;

namespace Lrw.Script.Enemy.EnemySkills
{
    public class GGHSkill : AbstractSkillModule
    {
        private float _coolTime;
        [SerializeField] private PoolManagerSO poolManagerSo;
        [SerializeField] private PoolItemSO poolItemSo;  
        
        [SerializeField] private float bulletSpeed;
        
        
        private void Update()
        {
            _coolTime -= Time.deltaTime;
        }
        
        public override bool CanUse(Transform target = null)
        {
            return _coolTime <= 0f;
        } 
        
        
        protected override void Use(Transform target = null)
        {
            _coolTime = 5f;
            EnemyBullet bullet = poolManagerSo.Pop<EnemyBullet>(poolItemSo);
            bullet.GameObject.SetActive(true);
            bullet.transform.position = transform.position;
            Vector2 vec = (target.position - transform.position).normalized * bulletSpeed; 
            bullet.Init(vec,Stat.Value);
            SetSkillEnd();
        }

        
    }
}