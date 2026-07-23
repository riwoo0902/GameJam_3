using UnityEngine;

namespace JJM.Scripts.Players
{
    public interface IPlayerRotation
    {
        Vector2 MouseRelativePosition { get; }
    }
}