using UnityEngine;

namespace Lrw.Script.Enemy
{
    public interface ISkillModule
    {
        float AttackDistance { get; }
        bool CanUse(Transform target = null);
        void Use(Transform target = null);
    }
}