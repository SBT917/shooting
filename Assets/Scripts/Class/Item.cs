using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//アイテムのクラス
public abstract class Item : MonoBehaviour
{
    [SerializeField]protected int point; //獲得した際に増えるポイント
    private float disapCnt; //消えるまでの時間
    private float speed; //プレイヤーの方向に吸い付くスピード
    private float range; //プレイヤーを感知する範囲
    protected Player player;
    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        disapCnt = 30.0f;
        speed = 10.0f;
        range = 2.0f;
        StartCoroutine(DisapItem());
    }

    void Update()
    {
        if(!player.gameObject.activeSelf) return; //プレイヤーが非アクティブになったら機能停止

        if(Vector3.Distance(player.transform.position, transform.position) < range){ //プレイヤーが近くに来たらアイテムがプレイヤーに吸い付く
            transform.LookAt(player.transform);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    //プレイヤーが入手した際の処理
    protected virtual void Get()
    {
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
