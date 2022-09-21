using System.Collections;
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

    void Awake()
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
        if (loadScene)
        {
            FindObjectOfType<TransitionController>().LoadScene(sceneToLoad);
        }
        Destroy(gameObject);
    }
}
