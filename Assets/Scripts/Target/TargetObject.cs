using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//ターゲットの状態
public enum TargetState
{
    Normal, //通常
    Break //破壊
}

//エネミーの目標となるターゲット
public class TargetObject : MonoBehaviour
{   
    private Player player;
    private GameManager gm;
    private AudioManager audioManager;

    public float maxHp = 100.0f; //最大HP
    public float hp; //現在のHP
    private TargetState state;
    private AudioSource audioSource;

    [SerializeField]private ParticleSystem hitParticle;
    [SerializeField]private ParticleSystem breakParticle;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        audioSource = GetComponent<AudioSource>();
        hp = maxHp;   
    }

    public TargetState GetState()
    {
        return state;
    }

    //ターゲットがダメージを受けた際の処理
    public void TakeDamage(float damage)
    {
        if(gm.GetState() != GameState.Game) return;
        audioManager.PlaySE("TargetDamage", audioSource);
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
        audioManager.PlaySE("TargetBreak", p.GetComponent<AudioSource>());
        p.Play();

        gameObject.SetActive(false);
    }
}
