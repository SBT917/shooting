using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Pool;

public class ShotLauncher : MonoBehaviour, IShotable
{
    [field: SerializeField]public Shot Shot { get; set; } //��������V���b�g
    public bool IsShooting { get; private set; } //�V���b�g�������Ă��邩�ǂ���
    public bool IsRecharging { get; private set; } //���`���[�W�����ǂ���
    public float RechargeCount { get; private set; } //���`���[�W����
    public IObjectPool<Shot> ShotPool { get; private set; } //�V���b�g�̃I�u�W�F�N�g�v�[��
    public int Amount { get; private set; } //�V���b�g�̎c�e��
    public int MaxAmount { get; private set; } //�V���b�g�̍ő�c�e��
    public Color ShotColor { get; private set; }

    public Action<int> onLaunch; //�ˌ����ɔ��s�����C�x���g
    public Action<float> onUpdateRecharge; //���`���[�W�i�s���ɔ��s�����C�x���g
    public Action<int> onFinishRecharge; //���`���[�W�������ɔ��s�����C�x���g

    [SerializeField]private Energy energy; //�G�l���M�[
    private bool isWait; //�ҋ@�����ǂ���
    private float waitCount; //�ҋ@����

    
    void Awake()
    {
        Init();
    }

    void Update()
    {        
        UpdateLaunch();
        UpdateRecharge();
    }

    void Init()
    {
        ShotPool = new ObjectPool<Shot>(
            OnCreateShot,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyShot,
            true,
            100,
            100
        );

        ShotColor = Shot.gameObject.GetComponent<Renderer>().sharedMaterial.color;
        Amount = Shot.ShotData.maxAmount;
        MaxAmount = Shot.ShotData.maxAmount;
    }

    private Shot OnCreateShot()
    {
        return Instantiate(Shot);
    }

    private void OnGetFromPool(Shot shot)
    {        
        shot.gameObject.SetActive(true);
        shot.ShotPool = ShotPool;
    }

    private void OnReleaseToPool(Shot shot)
    {
        shot.gameObject.SetActive(false);
    }

    private void OnDestroyShot(Shot shot)
    {
        Destroy(shot.gameObject);
    }

    public void StartLaunch()
    { 
        IsShooting = true;
    }

    public void UpdateLaunch()
    {
        if(!Wait()) Fire();
    }

    public void StopLaunch()
    {
        IsShooting = false;
    }

    public void StartRecharge()
    {
        if(IsRecharging) return;
        if(Amount >= MaxAmount) return;

        float useEnergy = Shot.ShotData.useEnergy - (Shot.ShotData.useEnergy * ((float)Amount / MaxAmount));
        if(energy.CurrentEnergy >= useEnergy)
        {
            RechargeCount = Shot.ShotData.rechargeTime;
            energy.DecreaseEnergy(useEnergy);
            IsRecharging = true;
        }

    }

    public void UpdateRecharge()
    {
        if(!IsRecharging) return;

        RechargeCount -= Time.deltaTime;
        onUpdateRecharge?.Invoke(RechargeCount);
        if (RechargeCount > 0) return;

        IsRecharging = false;
        Amount = MaxAmount;
        onFinishRecharge?.Invoke(Amount);
    }

    //�ˌ����̏���
    private void Fire()
    {
        if(!IsShooting) return;
        if(Amount <= 0) StartRecharge();
        if(IsRecharging) return;

        var shot = ShotPool.Get();
        shot.Init(transform);
        --Amount;
        onLaunch?.Invoke(Amount);

        waitCount = Shot.ShotData.rate;
        isWait = true;
    }

    //�ˌ��̑ҋ@����
    private bool Wait()
    {
        if (!isWait) return false;

        waitCount -= Time.deltaTime;
        if (waitCount <= 0)
        {
            isWait = false;
            return false;
        }

        return true;
    }


}
