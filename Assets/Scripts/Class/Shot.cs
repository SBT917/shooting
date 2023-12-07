using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//ショットのクラス
public abstract class Shot : MonoBehaviour
{
    protected Rigidbody rb; //リジットボディ
    protected GameObject player; //プレイヤー

    
    protected float disapCnt; //ショットが消滅するまでの時間
    protected ParticleSystem particle; //壁や敵に当たった時のパーティクル
    protected ParticleSystem.MainModule particleMain;

    protected AudioManager audioManager;

    [field: SerializeField]public ShotData ShotData { get; private set; } //ショットのデータをスクリプタブルオブジェクトから取得
    public IObjectPool<Shot> ShotPool { get; set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        particle = GetComponentInChildren<ParticleSystem>();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        particleMain = particle.main;
        particleMain.startColor = GetComponent<Renderer>().sharedMaterial.color;
    }

    protected virtual void OnEnable()
    {
        disapCnt = ShotData.disapCnt;
    }
    protected virtual void FixedUpdate()
    {
        Move();
    }

    //ショットが出現した際に呼ばれる
    public virtual void Init(Transform transform)
    {
        float randomRotate = Random.Range(-ShotData.blur, ShotData.blur); //弾のブレをshotDataのblurから取得し、その分ランダムに回転させてブレを表す。
        Quaternion rotate = Quaternion.Euler(0, randomRotate, 0);
        this.transform.localPosition = transform.position;
        this.transform.rotation = transform.rotation * rotate;
    }

    //移動処理
    protected virtual void Move()
    {
        if (disapCnt < 0) ShotPool.Release(this);

        disapCnt -= Time.deltaTime;
        rb.position += transform.forward * ShotData.moveSpeed * Time.deltaTime;

    }

    protected void GiveKnockBack(Collider other)
    {
        var knock = other.GetComponent<IKnockBackable>();
        if (knock != null)
        {
            knock.KnockBack(transform.forward);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            audioManager.PlaySE("HitEnemy", p.GetComponent<AudioSource>());
            p.Play();
            damageable.TakeDamage(ShotData.damage);
            GiveKnockBack(other);
            ShotPool.Release(this);
        }

        if (other.CompareTag("Wall"))
        {
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            audioManager.PlaySE("HitWall", p.GetComponent<AudioSource>());
            p.Play();
            ShotPool.Release(this);
        }

        
    }
}
