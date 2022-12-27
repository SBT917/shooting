using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//子オブジェクトにアタッチすると親オブジェクトの回転の影響を受けないようになる
public class FixChildRotation : MonoBehaviour
{
    Vector3 defaultRotation;

    void Awake ()
    {
        defaultRotation = transform.localRotation.eulerAngles;
    }
    
    void Update ()
    {
        Vector3 parent = transform.parent.transform.localRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(defaultRotation - parent);
    }


}

