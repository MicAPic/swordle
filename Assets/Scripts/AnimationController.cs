using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] 
    private BattleManager _battleManager;
    
    public void TriggerEnemyDamage()
    {
        _battleManager.enemyAnimator.SetBool("IsDead", true);
    }
}
