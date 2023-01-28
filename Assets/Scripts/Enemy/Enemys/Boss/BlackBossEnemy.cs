using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBossEnemy : BossEnemy
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

        EnemyShot shot = Instantiate(enemyShot, summonPos, Quaternion.identity);
        shot.transform.LookAt(player.transform);
        Vector3 randomRotate = new Vector3(0, Random.Range(-45, 45), 0);
        shot.transform.Rotate(randomRotate);
    }
}
