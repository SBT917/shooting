using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpContainer : MonoBehaviour
{
    public GameObject hpObj;

    public GameObject player;
    private int objAmount;
    private int nowHp;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        nowHp = player.GetComponent<Player>().maxHp;

        for(int i = 0; i < nowHp; ++i)
        {
            Instantiate(hpObj, transform);
        }
        objAmount = nowHp;
    }

    public void Relocation()
    {
        nowHp = player.GetComponent<Player>().hp;

        for(int i = 0; i < objAmount; ++i)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < nowHp; ++i)
        {
            Instantiate<GameObject>(hpObj, transform);
        }
        objAmount = nowHp;
    }
}
