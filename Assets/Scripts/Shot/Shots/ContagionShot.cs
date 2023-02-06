using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContagionShot : Shot
{
    [SerializeField]private Homing homing;
    [SerializeField]private float damageIncrease;
    private float hitCount = 0;

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy")){
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            audioManager.PlaySE("HitEnemy", p.GetComponent<AudioSource>());
            p.Play();
            other.GetComponent<Enemy>().TakeDamage(shotData.damage + (damageIncrease * hitCount));
            GiveKnockBack(other);
            homing.targetEnemys.Remove(other.gameObject);
            if(homing.targetEnemys.Count > 0){
                int index = UnityEngine.Random.Range(0, homing.targetEnemys.Count);
                transform.LookAt(homing.targetEnemys[index].transform);
                ++hitCount;
            }
            else{
                Destroy(gameObject);
            }  
        }

        if(other.CompareTag("Wall")){
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            audioManager.PlaySE("HitWall", p.GetComponent<AudioSource>());
            p.Play();
            Destroy(gameObject);
        }  
    }
}
