using Publics.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace JJM.Scripts.Player
{
    public class PlayerManager : MonoSingleton<PlayerManager>
    {
        [field: SerializeField] public Players.Player Player { get; set; }
    }
}