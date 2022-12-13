using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectShot : Shot
{
    private Vector3 direction;
    private Vector3 normal;
    private int reflectCount;

    [SerializeField]private int reflectMax;

    protected override void Awake()
    {
        reflectCount = 0;
        base.Awake();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")){
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            p.Play();
            other.GetComponent<Enemy>().TakeDamage(shotData.damage);
            Reflection();
        }
        
        if(other.CompareTag("Wall")){
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            p.Play();
            Reflection();
        }
    }

    private void Reflection()
    {
        int randomRotate = Random.Range(-120, -240);
        Vector3 rotate = new Vector3(0, randomRotate, 0);
        transform.Rotate(rotate);
        ++reflectCount;
        if(reflectCount >= reflectMax){
            Destroy(gameObject);
        }
    }
}
