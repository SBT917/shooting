using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Item : MonoBehaviour
{
    public int point;
    private float speed;
    protected Player player;
    
    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        speed = 20.0f;
    }

    void Update()
    {
        if(!player.gameObject.activeSelf) return;
        transform.LookAt(player.transform);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    protected virtual void Get()
    {
        player.nowPoint += point;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player")){
            Get();
        }    
    }
}
