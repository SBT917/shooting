using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEnemy : Enemy
{
    protected override void Awake()
    {
        maxHp = 3.0f;
        attackToPlayer = 1;
        attackToObject = 1.0f;
        normalSpeed = 1.0f;
        attackSpeed = 2.0f;
        socore = 100;
        searchAngle = 90.0f;

        base.Awake();
    }

    void Update()
    {
        if(GetState() != EnemyState.Death)
        {
            Act();
        }
    }

    protected override void Act()
    {
        NormalAction();
        AttackAction(20.0f, 3.0f);
        nav.SetDestination(target.transform.position);
    }
}
