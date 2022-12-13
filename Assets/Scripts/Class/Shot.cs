using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    protected Rigidbody rb;
    protected GameObject player;

    public ShotData shotData;
    protected float disapCnt;
    protected ParticleSystem particle;
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

    protected virtual void Move()
    {
        disapCnt -= Time.deltaTime;
        rb.position += transform.forward * shotData.moveSpeed * Time.deltaTime;
        if (disapCnt < 0)
            Destroy(gameObject);
    }

    public virtual void Instance()
    {
        player = GameObject.FindWithTag("Player");
        float randomRotate = Random.Range(-shotData.blur, shotData.blur);
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
