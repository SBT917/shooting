using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBag : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Act()
    {
        return;
    }

    protected override void Dead()
    {
        hp = enemyData.maxHp;
    }
}
