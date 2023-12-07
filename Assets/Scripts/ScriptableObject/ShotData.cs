using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ShotData")]
public class ShotData : ScriptableObject
{
    public string shotName; //ショット名
    public Sprite icon; //ショットのアイコン
    public Rarity rarity; //レアリティ
    public float moveSpeed; //移動速度
    public float damage; //ダメージ量
    public int maxAmount; //最大弾数
    public int shotNum; //一度に発射する弾数
    public float rechargeTime; //リチャージにかかる時間
    public float useEnergy; //リチャージに使用するEnergy
    public float rate; //連射速度
    public float disapCnt; //放たれてから消える時間
    public float blur; //弾のブレ
    public int price; //購入する時の値段
}

