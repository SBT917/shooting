using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Shot
{   [SerializeField]private int shotAmount;
    public override void Instance()
    {
        player = GameObject.FindWithTag("Player");
        float randomRotate;
        Quaternion rotate;

        for(int i = 0; i < shotAmount; ++i){
            randomRotate = Random.Range(-shotData.blur, shotData.blur);
            rotate = Quaternion.Euler(0, randomRotate, 0);
            Instantiate(gameObject, player.transform.position + new Vector3(0, offsetY, 0), player.transform.rotation * rotate);
        }
    }
}
