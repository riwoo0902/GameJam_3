using UnityEngine;
using UnityEngine.Events;

namespace JJM.Scripts
{
    public class StageCheckEvent : MonoBehaviour
    {
        public UnityEvent OnStageCheck;
        
        public void CheckStage(int stage)
        {
            if (stage == StageManager.Instance.CurrentStageNumber)
            {
                OnStageCheck?.Invoke();
            }
        }
    }
}