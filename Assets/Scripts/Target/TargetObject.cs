using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//ターゲットの状態
public enum TargetState{
        Normal, //通常
        Break //破壊
}

//エネミーの目標となるターゲット
public class TargetObject : MonoBehaviour
{   
    private Player player;
    private GameManager gm;

    public float maxHp = 100.0f; //最大HP
    public float hp; //現在のHP
    private TargetState state;

    [SerializeField]private CinemachineVirtualCamera targetCinemachine;
    [SerializeField]private CinemachineTargetGroup targetGroup;
    [SerializeField]private ParticleSystem hitParticle;
    [SerializeField]private ParticleSystem breakParticle;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        targetGroup.m_Targets[1].target = player.transform;
        hp = maxHp;   
    }

    void Update()
    {
        //プレイヤーがターゲットに接近したらターゲットを中心とするカメラに切り替える
        if(Vector3.Distance(transform.position, player.transform.position) < 10.0){
            targetCinemachine.gameObject.SetActive(true);
        }
        else if(Vector3.Distance(transform.position, player.transform.position) > 20.0){
            targetCinemachine.gameObject.SetActive(false);
        }
    }

    public TargetState GetState()
    {
        return state;
    }

    //ターゲットがダメージを受けた際の処理
    public void TakeDamage(float damage)
    {
        if(gm.GetState() != GameState.Game) return;
        hp -= damage;
        hitParticle.Play();
        if(hp <= 0)
        {
            Destroy();
        }
    }

    //ターゲットが破壊された際の処理
    protected virtual void Destroy()
    {
        state = TargetState.Break;

        var playerCinemachine = GameObject.FindWithTag("CinemachineVirtualCamera").GetComponent<CinemachineVirtualCamera>();
        playerCinemachine.Follow = transform;

        ParticleSystem p = Instantiate<ParticleSystem>(breakParticle, transform.position, Quaternion.identity);
        p.Play();

        gameObject.SetActive(false);
    }
}
