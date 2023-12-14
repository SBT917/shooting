using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

//エネミーの状態
public enum EnemyState
{
    Normal, //通常状態(目標に向かっている状態)
    Attack, //攻撃状態(プレイヤーに向かっている状態)
    Search, //サーチ状態(プレイヤーを見失っている状態)
    Death //倒された状態
}

//エネミーのクラス
public abstract class Enemy : MonoBehaviour, IDamageable
{
    public EnemyData enemyData; //エネミーデータの取得(スクリプタブルオブジェクト)
    protected Rigidbody rb;
    protected ParticleSystem deadParticle;
    protected NavMeshAgent nav;

    public float hp; //エネミーの現在のHP
    public Player player;
    public GameObject target; //エネミーが狙っているもの
    public GameObject[] targetObjects; //マップ上に存在する目標オブジェクト

    protected GameManager gameManager;
    protected EnemySpawner enemySpawner;
    protected AudioManager audioManager;
    [SerializeField] protected EnemyState state;
    private GameObject enemyCanvas;
    private GameObject hpBar;
    private float canvasDisapCnt;
    private AudioSource audioSource;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        deadParticle = GetComponentInChildren<ParticleSystem>();

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        targetObjects = GameObject.FindGameObjectsWithTag("Target");
        enemyCanvas = transform.Find("EnemyCanvas").gameObject;
        hpBar = enemyCanvas.transform.Find("EnemyHpBar").gameObject;
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        enemySpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        hp = enemyData.maxHp;
        hpBar.SetActive(false);
    }

    protected virtual void Update()
    {
        if (gameManager.GetState() == GameState.GameOver) return;
        if (state == EnemyState.Death) return;

        Act();
    }

    public void SetState(EnemyState newState)
    {
        state = newState;
    }

    public EnemyState GetState()
    {
        return state;
    }

    //エネミーの行動
    protected virtual void Act()
    {
        if (state == EnemyState.Death) return;

        NormalAction();
        AttackAction(enemyData.attackOutRange, enemyData.searchTime);
        nav.SetDestination(target.transform.position);
    }

    //プレイヤーに倒された時の処理
    protected virtual void Dead()
    {
        state = EnemyState.Death;
        ParticleSystem p = Instantiate<ParticleSystem>(deadParticle, transform.position, Quaternion.identity);

        audioManager.PlaySE("DeadEnemy", p.GetComponent<AudioSource>());
        p.Play();
        --enemySpawner.enemyCount;

        player.nowScore += enemyData.score;
        ItemDrop();
        gameObject.SetActive(false);
    }

    //エネミーが目標に到達して消滅する際の処理
    public virtual void Disappearing()
    {
        ParticleSystem p = Instantiate<ParticleSystem>(deadParticle, transform.position, Quaternion.identity);
        p.Play();
        --enemySpawner.enemyCount;

        gameObject.SetActive(false);
    }

    //通常状態の行動
    protected virtual void NormalAction()
    {
        if (state == EnemyState.Normal)
        {
            nav.speed = enemyData.normalSpeed;

            float[] distances = new float[targetObjects.Length]; //各ターゲットオブジェクトまでの距離を格納する
            for (int i = 0; i < targetObjects.Length; ++i)
            {
                distances[i] = Vector3.Distance(targetObjects[i].transform.position, transform.position);
            }
            float min = distances.Min(); //配列の一番小さい値を代入
            int minIndex = Array.IndexOf(distances, min); //一番小さい値のインデックスを代入
            target = targetObjects[minIndex]; //一番距離が近いオブジェクトをターゲットに設定する。
        }
    }

    //攻撃状態の行動
    protected virtual void AttackAction(float outRange, float searchTime)
    {
        if (state == EnemyState.Attack)
        {
            nav.speed = enemyData.attackSpeed;
            target = player.gameObject;

            if (CheckDistance(player.gameObject) > outRange || player.GetState() == PlayerState.Invisible)
            { //プレイヤーが範囲外に行くか、透明状態になったらサーチ状態に移行
                SetState(EnemyState.Search);
                StartCoroutine(SearchCo(searchTime));
            }
        }
    }

    //サーチ状態のコルーチン
    protected IEnumerator SearchCo(float time)
    {
        float count = time * 10;
        nav.speed = 0.0f;
        while (state == EnemyState.Search)
        {
            yield return new WaitForSeconds(0.1f);
            count--;
            if (count < 0) //サーチ状態のままカウントが0になればサーチ状態終了(通常状態に戻る)
            {
                SetState(EnemyState.Normal);
                yield break;
            }
        }
    }

    //引数targetとの距離を返す
    public float CheckDistance(GameObject target)
    {
        Vector3 yPos = target.transform.position;
        Vector3 mPos = this.transform.position;

        return Vector3.Distance(yPos, mPos);
    }

    //ダメージを受けた際の処理
    public virtual void TakeDamage(float damage)
    {
        if (state == EnemyState.Death) return;
        hp -= damage;
        SetState(EnemyState.Attack);
        StartCoroutine(CanvasAvtiveCo());

        if (hp <= 0)
        {
            Dead();
        }
    }

    //アイテムのドロップ制御
    protected void ItemDrop()
    {
        int randValue = UnityEngine.Random.Range(0, 100);
        if (enemyData.itemDropRatio < randValue) return;
        Instantiate(enemyData.dropItem, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.identity);
    }

    //プレイヤーからダメージを受けた際、頭上にHPバーを表示する
    protected IEnumerator CanvasAvtiveCo()
    {
        canvasDisapCnt = 10.0f;

        if (!hpBar.activeSelf)
        {
            hpBar.SetActive(true);

            while (canvasDisapCnt > 0.0f) //一定時間ダメージを受けなければHPバーは消える
            {
                canvasDisapCnt -= 1.0f;
                yield return new WaitForSeconds(1.0f);
            }

            hpBar.SetActive(false);
        }
        else
        {
            yield break;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamageable damageable))
        {
            switch (other.tag)
            {
                case "Player":
                    damageable.TakeDamage(enemyData.attackToPlayer);
                    break;
                case "Target":
                    damageable.TakeDamage(enemyData.attackToObject);
                    Disappearing();
                    break;
                default: 
                    break;
            }
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            switch (other.tag)
            {
                case "Player":
                    damageable.TakeDamage(enemyData.attackToPlayer);
                    break;
                default:
                    break;
            }
        }
    }
}
