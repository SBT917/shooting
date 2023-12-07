using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingShot : Shot
{
    [SerializeField]private Homing homing;
    
    protected override void Move()
    {   
        if(homing.targetEnemy != null){
            transform.LookAt(homing.targetEnemy.transform);
        }
        disapCnt -= Time.deltaTime;
        transform.position += transform.forward * ShotData.moveSpeed * Time.deltaTime;
        if (disapCnt < 0)
            Destroy(gameObject);
    }
}
