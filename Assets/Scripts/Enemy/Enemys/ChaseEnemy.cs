using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : Enemy
{
    protected override void Awake()
    {
        state = EnemyState.Attack;
        base.Awake();
    }

    protected override void NormalAction()
    {
        return;
    }

    protected override void AttackAction(float outRange, float searchTime)
    {
        if(state == EnemyState.Attack){
            nav.speed = enemyData.attackSpeed;
            target = player.gameObject;
        }
    }
}
