using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float maxHp;
    public int attackToPlayer;
    public float attackToObject;
    public float normalSpeed;
    public float attackSpeed;
    public int score;
    public float searchAngle;
    public float attackOutRange;
    public float searchTime;
    public int itemDropRatio;
    public GameObject[] dropItems;
}
