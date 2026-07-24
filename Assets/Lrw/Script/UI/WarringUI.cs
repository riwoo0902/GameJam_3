using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Lrw.Script.UI
{
    [RequireComponent(typeof(Image))]
    public class WarringUI : MonoBehaviour
    {
        private readonly int _fadeTime = Shader.PropertyToID("_FadeTime");
        private Image _image;
        private Material _material;

        [SerializeField] private float fadeTime =  3;
        private void Awake()
        {
            _image = GetComponent<Image>();
            _material = _image.material;
        }


        public void Fade()
        {
            StartCoroutine(FadeEnumerator());
        }

        private IEnumerator FadeEnumerator()
        {
            float currentTime = Time.time;
            float deltaTime = Time.time - currentTime;
            while (deltaTime < fadeTime)
            {
                deltaTime = Time.time - currentTime;
                _material.SetFloat(_fadeTime,deltaTime);
                yield return null;
            }
            _material.SetFloat(_fadeTime,0);
        }
    }
}