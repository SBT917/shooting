using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ShotData")]
public class ShotData : ScriptableObject
{
    public string shotName;
    public Rarity rarity;
    public float moveSpeed;
    public float damage;
    public float useEnergy;
    public float rate;
    public float disapCnt;
    public float blur;
    public int price;
}

