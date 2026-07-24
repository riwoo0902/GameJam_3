using DevLib.EventChannelSystem;
using DevLib.SoundSystem.Runtime;
using UnityEngine;

namespace Lrw.Script.UI
{
    public class SFXPlayer : MonoBehaviour
    {
        [SerializeField] private SoundClipSO soundClip;
        
        [ContextMenu("Play")]
        public void Play()
        {
            EventBus<PlaySoundEvents>.Invoke(SoundEvents.PlaySound.Init(transform.position, soundClip));
        }
        
        
    }
}