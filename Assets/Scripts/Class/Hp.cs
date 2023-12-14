using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    [field: SerializeField] public float MaxHp { get; private set; } //Hp�̍ő�l
    [field: SerializeField] public float CurrentHp { get; private set; } //���݂�Hp
    public Action<float> onUpdateHp; //Hp���ϓ��������ɌĂ΂��C�x���g
    public Action onHpZero; //Hp���[���ɂȂ������ɌĂ΂��C�x���g

    void Awake()
    {
        CurrentHp = MaxHp;
    }

    //Hp�����炷
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

    //Hp�𑝂₷
    public void IncreaseHp(float amount)
    {
        CurrentHp += amount;
        if (CurrentHp > MaxHp) CurrentHp = MaxHp;
        onUpdateHp?.Invoke(CurrentHp);
    }
}
