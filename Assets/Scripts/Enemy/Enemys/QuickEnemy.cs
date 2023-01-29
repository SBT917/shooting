using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }
    
    protected override void Update()
    {
        base.Update();
    }

    protected override void AttackAction(float outRange, float searchTime)
    {
        return;
    }

    public override void TakeDamage(float damage)
    {
        if(state == EnemyState.Death) return;
        hp -= damage;
        StartCoroutine(CanvasAvtiveCo());

        if(hp <= 0)
        {
            Dead();
        }
    }
}
