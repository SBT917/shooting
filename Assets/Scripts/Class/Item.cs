using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//アイテムのクラス
public class Item : MonoBehaviour
{
    public int point; //獲得した際に増えるポイント
    protected Player player;
    protected float disapCnt; //消えるまでの時間
    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        disapCnt = 30.0f;
        StartCoroutine(DisapItem());
    }

    void Update()
    {
        if(!player.gameObject.activeSelf) return; //プレイヤーが非アクティブになったら機能停止
    }

    //プレイヤーが入手した際の処理
        protected virtual void Get()
    {
        player.nowPoint += point;
        gameObject.SetActive(false);
    }

    //アイテムが時間切れで消滅するまでの処理
    private IEnumerator DisapItem()
    {
        yield return new WaitForSeconds(disapCnt);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player")){
            Get();
        }    
    }

    
}
