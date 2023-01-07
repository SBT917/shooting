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
        Vector3 summonPos = transform.localPosition + transform.forward;
        summonPos.y = 0.75f;

        Quaternion rotate = Quaternion.Euler(0, 10, 0);
        Instantiate(enemyShot, summonPos, transform.rotation * rotate);
        Instantiate(enemyShot, summonPos, transform.rotation * Quaternion.Inverse(rotate));
        Instantiate(enemyShot, summonPos, transform.rotation);
    }
}
