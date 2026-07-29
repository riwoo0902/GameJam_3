using UnityEngine;

namespace Lrw.Script.Enemy
{
    public interface ISkillModule
    {
        float AttackDistance { get; }
        bool SkillEnd { get; }
        bool CanUse(Transform target = null);
        void SkillUse(Transform target = null);
    }
}