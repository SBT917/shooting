using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Pool;

public class ShotLauncher : MonoBehaviour, IShotable
{
    [field: SerializeField]public Shot Shot { get; set; } //ê∂ê¨Ç∑ÇÈÉVÉáÉbÉg
    public bool IsShooting { get; private set; }
    public IObjectPool<Shot> ShotPool { get; private set; }

    private bool isWait;
    private float waitCount;

    void Awake()
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
    }

    void Update()
    {        
        UpdateLaunch();
    }

    private Shot OnCreateShot()
    {
        return Instantiate(Shot);
    }

    private void OnGetFromPool(Shot shot)
    {
        shot.gameObject.SetActive(true);
        shot.ShotPool = ShotPool;
        shot.Init(transform);
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
        isWait = false;
    }

    public void UpdateLaunch()
    {
        if (!IsShooting) return;

        if (isWait)
        {
            waitCount -= Time.deltaTime;
            if (waitCount < 0) isWait = false;
            return;
        }


        ShotPool.Get();
        waitCount = Shot.ShotData.rate;
        isWait = true;
    }

    public void StopLaunch()
    {
        IsShooting = false;
    }

}
