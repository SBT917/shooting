using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float maxHp; //最大HP
    public int attackToPlayer; //プレイヤーに対する攻撃力
    public float attackToObject; //目標オブジェクトに対する攻撃力
    public float normalSpeed; //通常時のスピード
    public float attackSpeed; //攻撃時のスピード
    public int score; //倒した時にもらえるスコア値
    public float searchAngle; //プレイヤーを察知できる範囲
    public float attackOutRange; //プレイヤーを見失う距離
    public float searchTime; //プレイヤーを見失った時にどのくらいサーチ状態を維持するか
    public int itemDropRatio; //アイテムがドロップする割合
    public GameObject dropItem; //ドロップするアイテム
}
