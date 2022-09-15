using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class TransitionController : MonoBehaviour
    {
        private static TransitionController _instance;
        public Material transitionMaterial;
        private bool _transitionStarted;

        void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
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
        private float _maxPixelSize = 0.15f;
        private float _transitionStep = 0.002f;
        private string _pixelSize = "_PixelSize";

        IEnumerator SceneTransition(string sceneName)
        {
            _transitionStarted = true;
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
        }
    }
}
