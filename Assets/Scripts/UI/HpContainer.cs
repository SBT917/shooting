using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//プレイヤーの現在のHPを示すUIの制御
public class HpContainer : MonoBehaviour
{
    public GameObject hpObj; //HPのUIオブジェクト

    public Player player;
    private int objAmount; //現在のhpObjの個数
    private int nowHp; //現在のプレイヤーのHP

    [SerializeField]private Sprite topFrame;

    void Start()
    {
        nowHp = player.maxHp;

        for(int i = 0; i < nowHp + 1; ++i) //ここで再配置(UIの先頭にフタを設置するため+1)
        {
            GameObject go = Instantiate<GameObject>(hpObj, transform);

            if(i == player.maxHp){ //最終ループの時にSpriteをフタのSpriteに変えて、青いImageと背景を削除
                Destroy(go.transform.GetChild(0).gameObject);
                Destroy(go.transform.GetChild(1).gameObject);
                go.transform.GetChild(2).GetComponent<Image>().sprite = topFrame;
            } 
        }
        objAmount = nowHp + 1;
    }

    //主にHPが増えた時に行う再配置
    public void Relocation()
    {
        nowHp = player.hp;

        for(int i = 0; i < objAmount; ++i) //一回全てのオブジェクトを削除
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < nowHp + 1; ++i) //ここで再配置(UIの先頭にフタを設置するため+1)
        {
            GameObject go = Instantiate<GameObject>(hpObj, transform);

            if(i == player.maxHp){ //最終ループの時にSpriteをフタのSpriteに変えて、青いImageと背景を削除
                Destroy(go.transform.GetChild(0).gameObject);
                Destroy(go.transform.GetChild(1).gameObject);
                go.transform.GetChild(2).GetComponent<Image>().sprite = topFrame;
            } 
            
        }
        objAmount = nowHp + 1;
    }

    //ダメージを食らった時にHPの表示を減らす処理
    public void TakeDamage()
    {
        int nowIndex = player.hp;
        transform.GetChild(nowIndex).GetChild(1).gameObject.SetActive(false); //現在のHP番目の子オブジェクトの青いImageを無効化する
    }
}
