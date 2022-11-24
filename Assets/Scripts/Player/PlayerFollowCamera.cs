using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distance;
    [SerializeField] private Quaternion Rotation;
    void LateUpdate()
    {
        transform.rotation = Rotation;
        transform.position = player.position - transform.rotation * Vector3.forward * distance;
    }
}
