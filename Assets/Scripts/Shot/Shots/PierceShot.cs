using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceShot : Shot
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")){
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            audioManager.PlaySE("HitEnemy", p.GetComponent<AudioSource>());
            p.Play();
            other.GetComponent<Enemy>().TakeDamage(shotData.damage);
            GiveKnockBack(other);
        }
        
        if(other.CompareTag("Wall")){
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            audioManager.PlaySE("HitWall", p.GetComponent<AudioSource>());
            p.Play();
            Destroy(gameObject);
        }
    }
}
