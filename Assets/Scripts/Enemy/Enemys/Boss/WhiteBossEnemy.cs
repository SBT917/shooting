using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBossEnemy : BossEnemy
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
        int randomRotate = Random.Range(-45, 45);

        for(int i = 0; i < 9; ++i){
            Vector3 rotate = transform.eulerAngles;
            rotate.y += (40 * i) + randomRotate;
            EnemyShot shot = Instantiate(enemyShot, summonPos, Quaternion.Euler(rotate));
        }
    }
}
