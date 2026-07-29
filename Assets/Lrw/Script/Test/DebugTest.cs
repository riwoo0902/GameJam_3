using UnityEngine;

namespace Lrw.Script.Test
{
    public class DebugTest : MonoBehaviour
    {
        [SerializeField] private string text;
        
        
        public void TestDebug()
        {
            Debug.Log(text);
        }
        
        
    }
}