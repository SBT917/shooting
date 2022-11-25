using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    private Enemy parentEnemy;
    private Slider slider;

    void Start()
    {
        parentEnemy = transform.root.gameObject.GetComponent<Enemy>();
        slider = GetComponent<Slider>();
        slider.maxValue = parentEnemy.enemyData.maxHp;
    }

    void Update()
    {
        slider.value = parentEnemy.hp;
    }
}
