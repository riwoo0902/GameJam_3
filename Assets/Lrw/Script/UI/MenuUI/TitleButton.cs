using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lrw.Script.UI.MenuUI
{
    [RequireComponent(typeof(Button))]
    public class TitleButton : MonoBehaviour
    {
        private Button _button;
        [SerializeField] private string titleSceneName = "Main";

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Click);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(Click);
        }

        private void Click()
        {
            SceneManager.LoadScene(titleSceneName);
        }
    }
}