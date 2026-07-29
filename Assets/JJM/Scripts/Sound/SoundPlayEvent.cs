using DevLib.EventChannelSystem;
using DevLib.SoundSystem.Runtime;
using UnityEngine;

namespace JJM.Scripts.Sound
{
    public class SoundPlayEvent : MonoBehaviour
    {
        public void PlaySound(SoundClipSO sound)
        {
            EventBus<PlaySoundEvents>.Invoke(SoundEvents.PlaySound.Init(transform.position, sound));
        }        
        
        public void StopSound(int num)
        {
            EventBus<StopSoundEvent>.Invoke(SoundEvents.StopSound.Init(num));
        }
    }
}