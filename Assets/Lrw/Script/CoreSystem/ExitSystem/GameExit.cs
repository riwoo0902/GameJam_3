using UnityEngine;

namespace Lrw.Script.CoreSystem.ExitSystem
{
    public static class GameExit
    {
        public static void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}