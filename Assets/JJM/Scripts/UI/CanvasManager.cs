using DevLib.AnimatorSystem;
using Publics.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JJM.Scripts.UI
{
    [RequireComponent(typeof(Animator))]
    public class CanvasManager : MonoSingleton<CanvasManager>
    {
        [Header("PowerUp Screen")]
        [SerializeField] private GameObject powerUpScreen;

        [Header("Animation")]
        [SerializeField] private HashDataSO powerUpStart;
        [SerializeField] private HashDataSO powerUpEnd;

        [Header("Panels")]
        [SerializeField] private PowerUpPanel[] powerUpPanels;

        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (Keyboard.current == null)
            {
                return;
            }

            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                PowerUpPlay();
            }
        }

        public void PowerUpPlay()
        {
            if (powerUpScreen == null)
            {
                Debug.LogError("PowerUpScreen이 연결되지 않았습니다.");
                return;
            }

            // 먼저 활성화해야 자식 패널 Animator.Play가 정상 실행됨
            powerUpScreen.SetActive(true);

            // 이전에 구매해서 Y = 0이 된 패널들을 초기화
            foreach (PowerUpPanel panel in powerUpPanels)
            {
                if (panel == null)
                {
                    continue;
                }

                panel.ResetPowerUpPanel();
            }

            // 화면 전체의 Start 애니메이션 실행
            if (_animator.isActiveAndEnabled)
            {
                _animator.Play(powerUpStart.HashValue, 0, 0f);
            }
        }

        public void PowerUpEnd()
        {
            if (!_animator.isActiveAndEnabled)
            {
                return;
            }

            _animator.Play(powerUpEnd.HashValue, 0, 0f);
        }

        // PowerUpEnd 애니메이션 마지막 Animation Event에서 호출
        public void DisablePowerUpScreen()
        {
            if (powerUpScreen != null)
            {
                powerUpScreen.SetActive(false);
            }
        }
    }
}