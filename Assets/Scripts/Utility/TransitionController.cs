using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class TransitionController : MonoBehaviour
    {
        public Material transitionMaterial;
        private bool _transitionStarted;
        private AudioSource _audioSource;

        void Awake()
        {
            _audioSource = FindObjectOfType<AudioSource>();
            StartCoroutine(AudioFade(1, 1));
            DontDestroyOnLoad(gameObject);
        }

        public void LoadScene(string sceneName)
        {
            if (!_transitionStarted)
            {
                StartCoroutine(SceneTransition(sceneName));
            }
        }

        // constants used in Pixelize.shader:
        private float _minPixelSize = 0.0001f;
        private float _maxPixelSize = 0.13f;
        private float _transitionStep = 0.002f;
        private string _pixelSize = "_PixelSize";

        IEnumerator SceneTransition(string sceneName)
        {
            _transitionStarted = true;
            StartCoroutine(AudioFade(0.6f, 0));
            yield return null;

            var size = _minPixelSize;
            while (size < _maxPixelSize)
            {
                size += _transitionStep;
                // Debug.Log(size);
                transitionMaterial.SetFloat(_pixelSize, size);
                yield return null;
            }
            
            SceneManager.LoadScene(sceneName);
            yield return null;
            
            while (size > _minPixelSize)
            {
                size -= _transitionStep;
                // Debug.Log(size);
                transitionMaterial.SetFloat(_pixelSize, size);
                yield return null;
            }
            
            _transitionStarted = false;
            Destroy(gameObject);
        }
        
        public IEnumerator AudioFade(float duration, float targetVolume)
        {
            var currentTime = 0.0f;
            var start = _audioSource.volume;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                _audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
        }
    }
}
