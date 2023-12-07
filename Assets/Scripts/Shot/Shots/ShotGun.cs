using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Shot
{   
    [SerializeField]private int shotAmount;

    public override void Init(Transform transform)
    {

        float randomRotate;
        Quaternion rotate;

        randomRotate = Random.Range(-ShotData.blur, ShotData.blur);
        rotate = Quaternion.Euler(0, randomRotate, 0);
        this.transform.position = transform.localPosition;
        this.transform.rotation = transform.rotation * rotate;


        for (int i = 0; i < shotAmount - 1; ++i)
        {
            randomRotate = Random.Range(-ShotData.blur, ShotData.blur);
            rotate = Quaternion.Euler(0, randomRotate, 0);
            var s = ShotPool.Get();
            s.transform.position = transform.localPosition;
            s.transform.rotation = transform.rotation * rotate;
        }

        
    }
}
