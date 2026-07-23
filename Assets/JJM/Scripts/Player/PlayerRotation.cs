using DevLib.AnimatorSystem;
using DevLib.ModuleSystem;
using Publics.Agent;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JJM.Scripts.Player
{
    public class PlayerRotation : Module, IPlayerRotation
    {
        [SerializeField] private Camera mainCamera;

        [SerializeField] private HashDataSO inputXHashData;
        [SerializeField] private HashDataSO inputYHashData;
        
        public Vector2 MouseRelativePosition { get; private set; }
        
        private IRenderer _renderer;
        
        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _renderer = _owner.GetModule<IRenderer>(); 

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        private void Update()
        {
            if (_owner == null || mainCamera == null || Mouse.current == null)
            {
                return;
            }

            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

            float cameraDistance = Mathf.Abs(
                mainCamera.transform.position.z -
                _owner.transform.position.z
            );

            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(
                new Vector3(
                    mouseScreenPosition.x,
                    mouseScreenPosition.y,
                    cameraDistance
                )
            );

            MouseRelativePosition =
                (Vector2)mouseWorldPosition -
                (Vector2)_owner.transform.position;
            
            _renderer.Animator.SetFloat(inputXHashData.HashValue, MouseRelativePosition.x);
            _renderer.Animator.SetFloat(inputYHashData.HashValue, MouseRelativePosition.y);
        }
    }
}