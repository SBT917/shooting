using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//プレイヤーの現在のHPを示すUIの制御
public class HpContainer : MonoBehaviour
{
    public GameObject hpObj; //HpのUIオブジェクト

    [SerializeField] private Hp hp;
    private int objAmount; //現在のhpObjの個数
    private int currentHp; //現在のHp

    [SerializeField]private Sprite topFrame;

    void Start()
    {
        currentHp = (int)hp.MaxHp;

        for(int i = 0; i < currentHp + 1; ++i) //ここで再配置(UIの先頭にフタを設置するため+1)
        {
            GameObject go = Instantiate(hpObj, transform);

            if(i == hp.MaxHp){ //最終ループの時にSpriteをフタのSpriteに変えて、青いImageと背景を削除
                Destroy(go.transform.GetChild(0).gameObject);
                Destroy(go.transform.GetChild(1).gameObject);
                go.transform.GetChild(2).GetComponent<Image>().sprite = topFrame;
            } 
        }
        objAmount = currentHp + 1;

        hp.onUpdateHp += Decrease;
    }

    //主にHPが増えた時に行う再配置
    public void Relocation(float amount)
    {
        currentHp = (int)amount;

        for(int i = 0; i < objAmount; ++i) //一回全てのオブジェクトを削除
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < currentHp + 1; ++i) //ここで再配置(UIの先頭にフタを設置するため+1)
        {
            GameObject go = Instantiate<GameObject>(hpObj, transform);

            if(i == hp.MaxHp){ //最終ループの時にhpObjのSpriteをフタのSpriteに変えて、青いImageと背景を削除
                Destroy(go.transform.GetChild(0).gameObject);
                Destroy(go.transform.GetChild(1).gameObject);
                go.transform.GetChild(2).GetComponent<Image>().sprite = topFrame;
            } 
            
        }
        objAmount = currentHp + 1;
    }

    //ダメージを食らった時にHPの表示を減らす処理
    public void Decrease(float currentHp)
    {
        int nowIndex = (int)currentHp;
        transform.GetChild(nowIndex).GetChild(1).gameObject.SetActive(false); //現在のHP番目の子オブジェクトの青いImageを無効化する
    }
}
