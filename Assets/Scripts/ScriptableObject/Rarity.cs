using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ショットのレアリティデータを格納する
[CreateAssetMenu(menuName = "ScriptableObject/Rarity")]
public class Rarity : ScriptableObject
{
    public string rarityName; //レアリティ名
    public Color color; //色
}
