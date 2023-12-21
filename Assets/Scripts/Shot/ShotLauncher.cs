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
    [field: SerializeField]public Shot Shot { get; set; } //生成するショット
    public bool IsShooting { get; private set; } //ショットを撃っているかどうか
    public bool IsRecharging { get; private set; } //リチャージ中かどうか
    public float RechargeCount { get; private set; } //リチャージ時間
    public IObjectPool<Shot> ShotPool { get; private set; } //ショットのオブジェクトプール
    public int Amount { get; private set; } //ショットの残弾数
    public int MaxAmount { get; private set; } //ショットの最大残弾数
    public Color ShotColor { get; private set; }

    public Action<int> onLaunch; //射撃時に発行されるイベント
    public Action<float> onUpdateRecharge; //リチャージ進行時に発行されるイベント
    public Action<int> onFinishRecharge; //リチャージ完了時に発行されるイベント

    [SerializeField]private Energy energy; //エネルギー
    private bool isWait; //待機中かどうか
    private float waitCount; //待機時間

    
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

    //射撃時の処理
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

    //射撃の待機処理
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
