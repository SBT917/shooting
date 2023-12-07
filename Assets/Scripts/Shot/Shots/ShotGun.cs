using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Shot
{   [SerializeField]private int shotAmount;
    public override void Init(Transform transform)
    {
        //float randomRotate;
        //Quaternion rotate;

        //for(int i = 0; i < shotAmount; ++i){
        //    randomRotate = Random.Range(-ShotData.blur, ShotData.blur);
        //    rotate = Quaternion.Euler(0, randomRotate, 0);
        //    var s = ShotPool.Get();
        //    s.transform.position = transform.localPosition;
        //    s.transform.rotation = transform.rotation * rotate;
        //}
        base.Init(transform);
    }
}
