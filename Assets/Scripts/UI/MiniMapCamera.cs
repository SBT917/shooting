using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float height;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, height, player.position.z);  
    }
}
