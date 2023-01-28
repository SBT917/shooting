using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBossEnemy : BossEnemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void SummoningShot()
    {
        Vector3 summonPos = transform.localPosition;
        summonPos.y = 0.75f;

        for(int i = 0; i < 9; ++i){
            Vector3 rotate = transform.eulerAngles;
            rotate.y += 40 * i;
            EnemyShot shot = Instantiate(enemyShot, summonPos, Quaternion.Euler(rotate));
        }
    }
}
