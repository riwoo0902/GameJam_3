using System;
using Lrw.Script.Enemy;
using Unity.Behavior;
using UnityEngine;

namespace Lrw.Script.BT.Condition
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "CheckDistance", story: "[Enemy] to [Target] distance [Operator]", category: "Conditions", id: "1736442f762264e95d235f6218ad2655")]
    public partial class CheckDistanceCondition : Unity.Behavior.Condition
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [Comparison(comparisonType: ComparisonType.All)]
        [SerializeReference] public BlackboardVariable<ConditionOperator> Operator;

        public override bool IsTrue()
        {
            if (Enemy.Value == null || Target.Value == null)
            {
                Debug.LogWarning("CheckEnemy distance condition : 값이 할당되지 않아 항상 false");
                return false;
            }
            
            float distance = Vector3.Distance(Enemy.Value.transform.position, Target.Value.transform.position);
            float threshold = Enemy.Value.SkillModule.AttackDistance;
            
            return Operator.Value switch
            {
                ConditionOperator.Equal => Mathf.Approximately(distance, threshold),
                ConditionOperator.NotEqual => !Mathf.Approximately(distance, threshold),
                ConditionOperator.Greater => distance > threshold,
                ConditionOperator.Lower => distance < threshold,
                ConditionOperator.GreaterOrEqual => distance >= threshold,
                ConditionOperator.LowerOrEqual => distance <= threshold,
                _ => false
            };
        }
        
    }
}
