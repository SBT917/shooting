using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    [field: SerializeField] public float MaxHp { get; private set; } //Hpの最大値
    [field: SerializeField] public float CurrentHp { get; private set; } //現在のHp
    public Action<float> onUpdateHp; //Hpが変動した時に呼ばれるイベント
    public Action onHpZero; //Hpがゼロになった時に呼ばれるイベント

    void Awake()
    {
        CurrentHp = MaxHp;
    }

    //Hpを減らす
    public void DecreaseHp(float amount)
    {
        CurrentHp -= amount;
        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            onHpZero?.Invoke();
        }
        onUpdateHp?.Invoke(CurrentHp);
    }

    //Hpを増やす
    public void IncreaseHp(float amount)
    {
        CurrentHp += amount;
        if (CurrentHp > MaxHp) CurrentHp = MaxHp;
        onUpdateHp?.Invoke(CurrentHp);
    }
}
