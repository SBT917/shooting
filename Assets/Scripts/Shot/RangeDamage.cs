using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDamage : MonoBehaviour
{
    [HideInInspector]public ShotData shotData;
    void Awake()
    {
        StartCoroutine(ColliderDisabling());
    }

    private IEnumerator ColliderDisabling()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<SphereCollider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Enemy")){
            other.GetComponent<Enemy>().TakeDamage(shotData.damage);
        }    
    }
}
