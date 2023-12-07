using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TripleShot : Shot
{
    public override void Init(Transform transform)
    {
        Quaternion rotate = Quaternion.Euler(0, 10, 0);
        List<Shot> s = new List<Shot>();
       

        Instantiate(gameObject, transform.localPosition, transform.rotation * Quaternion.Inverse(rotate));
        Instantiate(gameObject, transform.localPosition, transform.rotation * rotate);
        Instantiate(gameObject, transform.localPosition, transform.rotation);
    }
}
