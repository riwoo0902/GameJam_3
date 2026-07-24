using System;
using DevLib.EventChannelSystem;
using DG.Tweening;
using LrwLib.UnityPosition;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lrw.Script.UI.MenuUI
{
    public class MenuUIManager : MonoBehaviour
    {
        [SerializeField] private UnityPos hidePos;
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private Ease ease;
        
        private Vector2 _showPos;
        
        private RectTransform _rectTransform;
        private void Awake()
        {
            _rectTransform = transform as RectTransform;
            Debug.Assert(_rectTransform != null,"Is Not UI");
            _showPos = _rectTransform.position;
            _rectTransform.position = hidePos;
            EventBus<MenuEvent>.Event += MenuUIHandle;
        }

        private void OnDestroy()
        {
            EventBus<MenuEvent>.Event -= MenuUIHandle;
        }

        private bool _active = false;
        private void MenuUIHandle(MenuEvent evt)
        {
            _active = !_active;
            _rectTransform.DOKill();
            if (_active)
            {
                _rectTransform.DOMove(_showPos, duration).SetEase(ease);
            }
            else
            {
                _rectTransform.DOMove(hidePos, duration).SetEase(ease);
            }
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                MenuEvent();
            }
        }
        
        public void MenuEvent() => EventBus<MenuEvent>.Invoke(UIEvents.Menu);
        
    }
}