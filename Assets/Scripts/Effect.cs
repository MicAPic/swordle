using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Utility;

public class Effect : MonoBehaviour
{
    [SerializeField] 
    private float time;
    [SerializeField] 
    private bool loadScene;
    [SerializeField] 
    private string sceneToLoad;

    void Start()
    {
        StartCoroutine(WaitBeforeCoroutine());
    }

    IEnumerator WaitBeforeCoroutine()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(DestructionCoroutine());
    }

    IEnumerator DestructionCoroutine()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        if (loadScene)
        {
            FindObjectOfType<TransitionController>().LoadScene(sceneToLoad);
        }
    }
}
