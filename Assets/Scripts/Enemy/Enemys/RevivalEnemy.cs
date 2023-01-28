using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivalEnemy : Enemy
{
    [SerializeField]private Enemy[] revivalEnemys;
    protected override void Dead()
    {
        state = EnemyState.Death;
        ParticleSystem p = Instantiate<ParticleSystem>(deadParticle, transform.position, Quaternion.identity);

        audioManager.PlaySE("DeadEnemy", p.GetComponent<AudioSource>());
        p.Play();

        player.nowScore += enemyData.score;
        ItemDrop();
        Revival();
        gameObject.SetActive(false);
    }

    private void Revival()
    {
        int value = Random.Range(0, revivalEnemys.Length);
        Enemy enemy = revivalEnemys[value];
        Instantiate<Enemy>(enemy, transform.position, transform.rotation);
    }
}
