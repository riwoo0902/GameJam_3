using DevLib.EventChannelSystem;
using DevLib.SoundSystem.Runtime;
using UnityEngine;

namespace Lrw.Script.UI
{
    public class BGMPlayer : MonoBehaviour
    {
        [SerializeField] private SoundClipSO soundClip;
        
        public void Awake()
        {
            EventBus<PlaySoundEvents>.Invoke(SoundEvents.PlaySound.Init(transform.position, soundClip,1));
        }
    }
}