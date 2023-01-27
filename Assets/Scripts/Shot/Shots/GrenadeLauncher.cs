using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Shot
{
    [SerializeField]private GameObject explosionEffect;
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();
        disapCnt = shotData.disapCnt;
    }

    protected override void Move()
    {
        if (disapCnt < 0) Explosion();
            
        disapCnt -= Time.deltaTime;
        rb.position += transform.forward * shotData.moveSpeed * Time.deltaTime;
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")){
            Explosion();
        }
        
        if(other.CompareTag("Wall")){
            Explosion();
        }
    }

    private void Explosion()
    {
        GameObject go = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        go.GetComponent<RangeDamage>().shotData = shotData;
        Destroy(gameObject);
    }
}
