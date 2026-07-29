using UnityEngine;

namespace DevLib.PolyNavMesh
{
    [CreateAssetMenu(fileName = "Nav agent data", menuName = "Lib/PolyNav/Agent data", order = 0)]
    public class NavAgentDataSO : ScriptableObject
    {
        [field: SerializeField] public float AgentRadius { get; private set; } = 0.5f;
    }
}