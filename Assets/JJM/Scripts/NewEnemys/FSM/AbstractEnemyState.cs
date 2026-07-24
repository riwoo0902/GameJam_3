using Agents;
using Agents.FSM;

namespace JJM.Scripts.NewEnemys.FSM
{
    public abstract class AbstractEnemyState : AgentState
    {
        protected NewEnemyController Enemy;
        
        protected AbstractEnemyState(Agent agent, int stateClipHash, int layerIndex) : base(agent, stateClipHash, layerIndex)
        {
            Enemy = agent.GetComponent<NewEnemyController>();
        }
    }
}