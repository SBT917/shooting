using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvisible : MonoBehaviour, IInvisible
{
    [SerializeField] private float useEnergy = 1.0f; //透明化中の際に消費し続けるEnergy
    [SerializeField] private float useEnergySpeed = 10.0f; //Energyを消費するスピード
    [SerializeField] private float speedMagnification = 2.0f; //透明化状態の移動速度倍率
    [SerializeField] private float inWallSpeedMagnification = 1.2f; //透明化かつ壁の中にいる時の移動速度倍率
    [SerializeField] private float inWallAndZeroEnergySpeedMagnification = 0.5f; //透明化かつ壁の中にいてEnergyが0の時の移動速度倍率

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

        energy.IsHealing = false; //透明化中はEnergyが回復しない
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

        energy.DecreaseEnergy(useEnergy * Time.deltaTime * useEnergySpeed); //Energyを消費
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
        energy.IsHealing = true; //透明化解除後はEnergyが回復する
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
