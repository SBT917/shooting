using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボスエネミーのクラス
public abstract class BossEnemy : Enemy
{
    [SerializeField]protected Enemy[] summonEnemys; //召喚するエネミー配列
    [SerializeField]protected EnemyShot enemyShot; //召喚するショット

    [SerializeField]protected int enemySummonCount; //エネミーを一度に召喚する数
    [SerializeField]protected float enemySummonSpan; //エネミーを召喚するスパン
    [SerializeField]protected float shotSummonSpan; //ショットを召喚するスパン

    [SerializeField]private int enemySummonCountMagnitude; //HPが半分になった後にエネミーの召喚数を増やす数
    [SerializeField]private float enemySummonSpanMagnitude; //HPが半分になった後にエネミーを召喚するスパンを短くする倍率

    [SerializeField]private float shotSummonSpanMagnitude; //HPが半分になった後にショットを召喚するスパンを短くする倍率

    [SerializeField]private ParticleSystem spawnParticle;
    private bool isShoting;
    private bool isSeriousMode;

    Coroutine summonEnemyCo;
    Coroutine summonShotCo;

    protected override void Awake()
    {
        summonEnemyCo = StartCoroutine(SummoningEnemyCo(enemySummonCount, enemySummonSpan));
        base.Awake();
    }

    protected override void Update() 
    {
        if(gameManager.GetState() == GameState.GameOver) return;
        if(state == EnemyState.Death) return;

        CheckHalfHp();
        base.Update();
    }

    
    protected override void AttackAction(float outRange, float searchTime)
    {
        if(state == EnemyState.Attack){
            nav.speed = enemyData.attackSpeed;
            target = player.gameObject;
            if(!isShoting){
                summonShotCo = StartCoroutine(SummoningShotCo(shotSummonSpan));
                isShoting = true;
            } 

            if(CheckDistance(player.gameObject) > outRange || player.GetState() == PlayerState.Invisible){ //プレイヤーが範囲外に行くか、透明状態になったらサーチ状態に移行
                SetState(EnemyState.Search);
                StopCoroutine(summonShotCo);
                isShoting = false;
                StartCoroutine(SearchCo(searchTime));
            }  
        }
    }

    //パーティクルを生成してから敵をスポーンさせるコルーチン
    private IEnumerator SpawnCo(Vector3 spawnPos, int value)
    {
        Instantiate(spawnParticle, spawnPos, Quaternion.identity);
        yield return new WaitForSeconds(spawnParticle.main.duration);
        Instantiate(summonEnemys[value], spawnPos, transform.rotation);
    }

    //ボスがエネミーを召喚するコルーチン
    private IEnumerator SummoningEnemyCo(int count, float span)
    {
        while(true){
            for(int i = 0; i < count; ++i){
                int randValue = Random.Range(0, summonEnemys.Length);
                Vector3 spawnPos = new Vector3(transform.position.x + Random.Range(-5, 5),
                                               transform.position.y, 
                                               transform.position.z + Random.Range(-5, 5));
                StartCoroutine(SpawnCo(spawnPos, randValue));
            }
            yield return new WaitForSeconds(span);
        }    
    }

    protected virtual void SummoningShot()
    {
        Vector3 summonPos = transform.localPosition + transform.forward;
        summonPos.y = 0.75f;
        Instantiate(enemyShot, summonPos, transform.rotation);
    }

    protected IEnumerator SummoningShotCo(float span)
    {
        while(true){
            yield return new WaitForSeconds(span);
            SummoningShot();
        }
    }

    //Hpが半分になったらボスの行動が激しくなる
    protected void CheckHalfHp()
    {
        if(isSeriousMode) return;

        if(hp <= enemyData.maxHp / 2){
            StopCoroutine(summonEnemyCo);
            summonEnemyCo = StartCoroutine(SummoningEnemyCo(enemySummonCount + enemySummonCountMagnitude , enemySummonSpan *= enemySummonSpanMagnitude));
            shotSummonSpan *= shotSummonSpanMagnitude;
            isSeriousMode = true;
        }
    }

    protected override void Dead()
    {
        state = EnemyState.Death;
        ParticleSystem p = Instantiate<ParticleSystem>(deadParticle, transform.position, Quaternion.identity);
        p.Play();

        player.nowScore += enemyData.score;
        ItemDrop();

        gameManager.BossDestroying();
        gameObject.SetActive(false);
    }
    
}
