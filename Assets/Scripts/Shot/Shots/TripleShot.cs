using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TripleShot : Shot
{
    public override void Instance()
    {
        player = GameObject.FindWithTag("Player");
        Quaternion rotate = Quaternion.Euler(0, 10, 0);

        Instantiate(gameObject, player.transform.position  + new Vector3(0, offsetY, 0), player.transform.rotation * Quaternion.Inverse(rotate));
        Instantiate(gameObject, player.transform.position  + new Vector3(0, offsetY, 0), player.transform.rotation * rotate);
        Instantiate(gameObject, player.transform.position  + new Vector3(0, offsetY, 0), player.transform.rotation);
    }
}
