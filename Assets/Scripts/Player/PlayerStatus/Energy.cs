using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Energy : MonoBehaviour
{
    
    [field: SerializeField] public float MaxEnergy { get; private set; }
    [field: SerializeField] private float HealSpeed { get; set; }
    public float CurrentEnergy { get; private set; }
    public bool IsHealing { get; set; }

    private float defaultEnergy;
    public Action<float> onUpdateEnergy;

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
