using DevLib.ModuleSystem;
using UnityEngine;

namespace Publics.Agent
{
    [RequireComponent(typeof(Animator))]
    public class AgentRenderer : Module, IRenderer
    {
        public Animator Animator {get; private set;}
        public override void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            Animator = GetComponent<Animator>();
        }
        
        public void PlayClip(int clipHash, float normalizedTime, float crossFadeDuration, int layerIndex = 0)
        {
            // Play, CrossFade, CrosseFadeFixedTime 
            //         정규화            조절
//            Animator.CrossFadeInFixedTime(clipHash, crossFadeDuration, layerIndex, normalizedTime);
        }

        public void SetFloat(int idHash, float value)
        {
            Animator.SetFloat(idHash, value);
        }
    }
}