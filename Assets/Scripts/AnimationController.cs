using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

public class AnimationController : MonoBehaviour
{
    [SerializeField] 
    private BattleManager battleManager;
    [SerializeField] 
    private TransitionController transitionController;
    private bool _canTrigger = true;
    
    public void TriggerEnemyDamage()
    {
        battleManager.enemyAnimator.SetBool("IsDead", true);
    }

    public void TriggerSceneChange(string sceneName)
    {
        transitionController.LoadScene(sceneName);
    }
    
    public void TriggerWarriorVfx()
    {
        Instantiate(battleManager.playerSfx);
    }

    public void TriggerEnemyVfx()
    {
        Instantiate(battleManager.enemyVfx);
    }

    public void TriggerVictorySfx()
    {
        if (_canTrigger)
        {
            StartCoroutine(FindObjectOfType<TransitionController>().AudioFade(1, 0));
            StartCoroutine(InstantiateWithDelay(1.2f));
            _canTrigger = false;
        }
    }

    IEnumerator InstantiateWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(battleManager.victorySfx);
    }
}
