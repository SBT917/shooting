using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    protected Rigidbody rb;
    protected GameObject player;

    public ShotData shotData;
    protected float disapCnt;
    protected float offsetY = 0.0f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        Instantiate(gameObject, player.transform.localPosition, player.transform.rotation * rotate);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")){
            Destroy(gameObject);
            other.GetComponent<Enemy>().TakeDamage(shotData.damage);
        }
        
        if(other.CompareTag("Wall")){
            Destroy(gameObject);
        }
    }
}
