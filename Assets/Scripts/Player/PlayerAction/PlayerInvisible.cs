using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvisible : MonoBehaviour, IInvisible
{
    [SerializeField] private float useEnergy = 1.0f; //���������̍ۂɏ��������Energy
    [SerializeField] private float useEnergySpeed = 10.0f; //Energy�������X�s�[�h
    [SerializeField] private float speedMagnification = 2.0f; //��������Ԃ̈ړ����x�{��
    [SerializeField] private float inWallSpeedMagnification = 1.2f; //���������ǂ̒��ɂ��鎞�̈ړ����x�{��
    [SerializeField] private float inWallAndZeroEnergySpeedMagnification = 0.5f; //���������ǂ̒��ɂ���Energy��0�̎��̈ړ����x�{��

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material invisibleMaterial;

    private IMoveable move;
    private Energy energy;
    private MeshRenderer mesh;
    private Collider coll;
    private bool inWall;
    private bool inWallCancell;
    private float defaultSpeed;

    public bool IsInvisible { get; set; }

    void Awake()
    {
        TryGetComponent(out move);
        TryGetComponent(out energy);
        mesh = GetComponentInChildren<MeshRenderer>();
        TryGetComponent(out coll); 
    }

    void Update()
    {
        UpdateInvisible();
    }

    public void StartInvisible()
    {
        if (IsInvisible) return;

        IsInvisible = true;

        energy.IsHealing = false; //����������Energy���񕜂��Ȃ�
        defaultSpeed = move.Speed;
        move.Speed *= speedMagnification;
        mesh.material = invisibleMaterial;
        coll.isTrigger = true;

        AudioManager.instance.PlaySE("InvisibleOn", audioSource);
    }

    public void UpdateInvisible()
    {
        if (!IsInvisible) return;

        if (energy.CurrentEnergy <= 0)
        {
            if (!inWall) EndInvisible();
            else move.Speed = defaultSpeed * inWallAndZeroEnergySpeedMagnification;
            return;
        }

        energy.DecreaseEnergy(useEnergy * Time.deltaTime * useEnergySpeed); //Energy������
        if(inWall) move.Speed = defaultSpeed * speedMagnification * inWallSpeedMagnification;
        else move.Speed = defaultSpeed * speedMagnification;

        if (!inWall && inWallCancell) { EndInvisible(); }
    }

    public void EndInvisible()
    {
        if(!IsInvisible) return;
        if (inWall)
        {
            inWallCancell = true;
            return;
        }

        IsInvisible = false;
        inWallCancell = false;
       
        move.Speed = defaultSpeed;
        energy.IsHealing = true; //�������������Energy���񕜂���
        mesh.material = defaultMaterial;
        coll.isTrigger = false;

        AudioManager.instance.PlaySE("InvisibleOff", audioSource);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            inWall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            inWall = false;
        }
    }
}
