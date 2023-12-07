using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Shot
{
    [SerializeField]private GameObject explosionEffect;
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();
        disapCnt = ShotData.disapCnt;
    }

    protected override void Move()
    {
        if (disapCnt < 0) Explosion();
            
        disapCnt -= Time.deltaTime;
        rb.position += transform.forward * ShotData.moveSpeed * Time.deltaTime;
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")){
            GiveKnockBack(other);
            Explosion();
        }
        
        if(other.CompareTag("Wall")){
            Explosion();
        }
    }

    private void Explosion()
    {
        GameObject go = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        go.GetComponent<RangeDamage>().shotData = ShotData;
        Destroy(gameObject);
    }
}
