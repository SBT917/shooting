using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Energy : MonoBehaviour
{
    
    [field: SerializeField] public float MaxEnergy { get; private set; } //エネルギーの最大値
    [field: SerializeField] private float HealSpeed { get; set; } //エネルギーの回復速度
    public float CurrentEnergy { get; private set; } //現在のエネルギー
    public bool IsHealing { get; set; } //回復中かどうか

    private float defaultEnergy; //エネルギーの最初期の値
    public Action<float> onUpdateEnergy; //エネルギーが変動した時に呼ばれるイベント

    void Awake()
    {
        CurrentEnergy = MaxEnergy;
        defaultEnergy = MaxEnergy;
        IsHealing = true;
    }

    void Update()
    {
        AutoHealing();
    }

    private void AutoHealing()
    {
        if (IsHealing && CurrentEnergy != MaxEnergy)
        {
            float amount = MaxEnergy / defaultEnergy;
            IncreaseEnergy(amount * HealSpeed * Time.deltaTime);
        }
    }

    //エネルギーを減らす
    public void DecreaseEnergy(float amount)
    {
        CurrentEnergy -= amount;
        if (CurrentEnergy < 0) CurrentEnergy = 0;
        onUpdateEnergy?.Invoke(CurrentEnergy);
    }

    //エネルギーを増やす
    public void IncreaseEnergy(float amount)
    {
        CurrentEnergy += amount;
        if (CurrentEnergy > MaxEnergy) CurrentEnergy = MaxEnergy;
        onUpdateEnergy?.Invoke(CurrentEnergy);
    }
}
