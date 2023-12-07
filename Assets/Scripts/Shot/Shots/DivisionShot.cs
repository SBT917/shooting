using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionShot : Shot
{
    [HideInInspector]public int divideCount;
    [HideInInspector]public int reflectCount;
    [SerializeField]private int divideMax;
    [SerializeField]private int reflectMax;
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")){
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            audioManager.PlaySE("HitEnemy", p.GetComponent<AudioSource>());
            p.Play();
            other.GetComponent<Enemy>().TakeDamage(ShotData.damage);
            GiveKnockBack(other);
            ShotDivide();
        }
        
        if(other.CompareTag("Wall")){
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            audioManager.PlaySE("HitWall", p.GetComponent<AudioSource>());
            p.Play();
            Reflection();
        }
    }

    private void ShotDivide()
    {
        if(divideCount >= divideMax) Destroy(gameObject);

        ++divideCount;
        for(int i = 0; i < 2; ++i){
            Vector3 rotate = new Vector3(0, (180 - 45) + (90 * i), 0);
            Vector3 position = transform.position - transform.forward;
            DivisionShot shot = Instantiate(gameObject, position, transform.rotation).GetComponent<DivisionShot>();
            shot.enabled = true;
            shot.transform.Rotate(rotate);
            shot.divideCount = this.divideCount;
        }
        Destroy(gameObject);
        
    }

    private void Reflection()
    {
        if(reflectCount >= reflectMax) Destroy(gameObject);

        ++reflectCount;
        int randomRotate = Random.Range(-120, -240);
        Vector3 rotate = new Vector3(0, randomRotate, 0);
        transform.Rotate(rotate);
    }
}
