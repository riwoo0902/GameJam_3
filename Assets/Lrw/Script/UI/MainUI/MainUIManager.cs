using System;
using DevLib.EventChannelSystem;
using Lrw.Script.CoreSystem.ExitSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lrw.Script.UI.MainUI
{
    public class MainUIManager : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button exitButton;
        
        [Header("StartButton")]
        [SerializeField] private string moveScene;
        
        
        private void Awake()
        {
            startButton.onClick.AddListener(StartButtonClick);
            menuButton.onClick.AddListener(MenuButtonClick);
            exitButton.onClick.AddListener(ExitButtonClick);
        }

        
        private void OnDestroy()
        {
            startButton.onClick.RemoveAllListeners();
            menuButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }

        private void StartButtonClick()
        {
            SceneManager.LoadScene(moveScene);
        }
        
        private void ExitButtonClick()
        {
            GameExit.Exit();
        }
        
        private void MenuButtonClick()
        {
            EventBus<MenuEvent>.Invoke(UIEvents.Menu);
        }
        
        
        
        
        
    }
}