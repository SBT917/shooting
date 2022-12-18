using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ショットのクラス
public class Shot : MonoBehaviour
{
    protected Rigidbody rb; //リジットボディ
    protected GameObject player; //プレイヤー

    public ShotData shotData; //ショットのデータをスクリプタブルオブジェクトから取得
    protected float disapCnt; //ショットが消滅するまでの時間
    protected ParticleSystem particle; //壁や敵に当たった時のパーティクル
    private ParticleSystem.MainModule particleMain;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        particle = GetComponentInChildren<ParticleSystem>();
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
        disapCnt -= Time.deltaTime;
        rb.position += transform.forward * shotData.moveSpeed * Time.deltaTime;
        if (disapCnt < 0)
            Destroy(gameObject);
    }

    //ショットを出現させる際の処理
    public virtual void Instance()
    {
        player = GameObject.FindWithTag("Player");
        float randomRotate = Random.Range(-shotData.blur, shotData.blur); //弾のブレをshotDataのblurから取得し、その分ランダムに回転させてブレを表す。
        Quaternion rotate = Quaternion.Euler(0, randomRotate, 0);
        Instantiate(gameObject, player.transform.localPosition + player.transform.forward, player.transform.rotation * rotate);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")){
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            p.Play();
            Destroy(gameObject);
            other.GetComponent<Enemy>().TakeDamage(shotData.damage);
        }
        
        if(other.CompareTag("Wall")){
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            p.Play();
            Destroy(gameObject);
        }
    }
}
