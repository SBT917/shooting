using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningGather : Shot
{
    [SerializeField]private GameObject lightningEffect;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();
        disapCnt = shotData.disapCnt;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")){
            SummonLightning(other.transform.position);
        }
    }

    private void SummonLightning(Vector3 position)
    {
        Vector3 summonPos = new Vector3(position.x, 10.0f, position.z);
        GameObject go = Instantiate(lightningEffect, summonPos, Quaternion.identity);
        go.GetComponent<RangeDamage>().shotData = shotData;
    }
}
