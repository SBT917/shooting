using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TripleShot : Shot
{
    public override void Init(Transform transform)
    {
        Quaternion rotate = Quaternion.Euler(0, 10, 0);

        base.Init(transform);

        var s1 = ShotPool.Get();
        s1.transform.position = transform.position;
        s1.transform.rotation = transform.rotation * Quaternion.Inverse(rotate);

        var s2 = ShotPool.Get();
        s2.transform.position = transform.position;
        s2.transform.rotation = transform.rotation * rotate;

        

    }
}
