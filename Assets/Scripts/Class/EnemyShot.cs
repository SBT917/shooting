using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyShot : Shot
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            Player player = other.GetComponent<Player>();

            if(player.GetState() == PlayerState.Invisible) return;

            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            p.Play();
            Destroy(gameObject);
            player.TakeDamage((int)shotData.damage);
        }

        if(other.CompareTag("Wall")){
            ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
            p.Play();
            Destroy(gameObject);
        } 
    }
}
