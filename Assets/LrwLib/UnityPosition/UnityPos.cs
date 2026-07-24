using System;
using UnityEngine;

namespace LrwLib.UnityPosition
{
    [Serializable]
    public struct UnityPos
    {
        [SerializeField] private Vector3 position;

        public Vector3 Position
        {
            get => position;
            set => position = value;
        }

        public UnityPos(Vector3 position)
        {
            this.position = position;
        }

        public static implicit operator Vector3(UnityPos unityPos) => unityPos.position;
        
        
    }
}
