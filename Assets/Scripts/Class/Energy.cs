using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Energy : MonoBehaviour
{
    
    [field: SerializeField] public float MaxEnergy { get; private set; } //�G�l���M�[�̍ő�l
    [field: SerializeField] private float HealSpeed { get; set; } //�G�l���M�[�̉񕜑��x
    public float CurrentEnergy { get; private set; } //���݂̃G�l���M�[
    public bool IsHealing { get; set; } //�񕜒����ǂ���

    private float defaultEnergy; //�G�l���M�[�̍ŏ����̒l
    public Action<float> onUpdateEnergy; //�G�l���M�[���ϓ��������ɌĂ΂��C�x���g

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

    //�G�l���M�[�����炷
    public void DecreaseEnergy(float amount)
    {
        CurrentEnergy -= amount;
        if (CurrentEnergy < 0) CurrentEnergy = 0;
        onUpdateEnergy?.Invoke(CurrentEnergy);
    }

    //�G�l���M�[�𑝂₷
    public void IncreaseEnergy(float amount)
    {
        CurrentEnergy += amount;
        if (CurrentEnergy > MaxEnergy) CurrentEnergy = MaxEnergy;
        onUpdateEnergy?.Invoke(CurrentEnergy);
    }
}
