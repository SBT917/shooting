using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ショットのクラス
public abstract class Shot : MonoBehaviour
{
    protected Rigidbody rb; //リジットボディ
    protected GameObject player; //プレイヤー

    public ShotData shotData; //ショットのデータをスクリプタブルオブジェクトから取得
    protected float disapCnt; //ショットが消滅するまでの時間
    protected ParticleSystem particle; //壁や敵に当たった時のパーティクル
    protected ParticleSystem.MainModule particleMain;

    protected AudioManager audioManager;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        particle = GetComponentInChildren<ParticleSystem>();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        particleMain = particle.main;
        particleMain.startColor = GetComponent<Renderer>().sharedMaterial.color;
        disapCnt = shotData.disapCnt;
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    //移動処理
    protected virtual void Move()
    {
        if (disapCnt < 0) Destroy(gameObject);
            
        disapCnt -= Time.deltaTime;
        rb.position += transform.forward * shotData.moveSpeed * Time.deltaTime;
        
    }

    //ショットを出現させる際の処理
    public virtual void Instance()
    {
        player = GameObject.FindWithTag("Player");
        float randomRotate = Random.Range(-shotData.blur, shotData.blur); //弾のブレをshotDataのblurから取得し、その分ランダムに回転させてブレを表す。
        Quaternion rotate = Quaternion.Euler(0, randomRotate, 0);
        Instantiate(gameObject, player.transform.localPosition + player.transform.forward, player.transform.rotation * rotate);
    }

    protected void GiveKnockBack(Collider other)
    {
        var knock = other.GetComponent<IKnockBackObject>();
        if(knock != null){
            knock.KnockBack(transform.forward);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")){
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            audioManager.PlaySE("HitEnemy", p.GetComponent<AudioSource>());
            p.Play();
            other.GetComponent<Enemy>().TakeDamage(shotData.damage);
            GiveKnockBack(other);
            Destroy(gameObject);    
        }
        
        if(other.CompareTag("Wall")){
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            audioManager.PlaySE("HitWall", p.GetComponent<AudioSource>());
            p.Play();
            Destroy(gameObject);
        }
    }
}
