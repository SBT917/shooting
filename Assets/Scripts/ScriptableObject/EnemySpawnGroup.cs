using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EnemySpawnGroup")]
public class EnemySpawnGroup : ScriptableObject
{
    public GameObject[] enemys;
}
