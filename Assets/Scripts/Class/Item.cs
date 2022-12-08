using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Item : MonoBehaviour
{
    public int point;
    protected Player player;
    protected float disapCnt;
    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        disapCnt = 30.0f;
        StartCoroutine(DisapItem());
    }

    void Update()
    {
        if(!player.gameObject.activeSelf) return;
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

    private IEnumerator DisapItem()
    {
        yield return new WaitForSeconds(disapCnt);
        Destroy(gameObject);
    }
}
