using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ショットスロットの状態
public enum SlotState
{
    Ready, //発射可能
    Wait, //発射不可能
    Recharging //リロード中
}

//プレイヤーが所持しているショットの情報を保存するクラス。この情報を基にショットを撃つ。
public class ShotSlot : MonoBehaviour
{
    public Shot shot; //スロットが持つショット
    public int shotAmount; //ショットの残弾数
    private Player player; //プレイヤー
    public Color shotColor; //ショットの色
    private ParticleSystem particle; //撃った時に発生させるパーティクル
    private ParticleSystem.MainModule particleMain; //パーティクルの値を制御
    [SerializeField]private SlotState state; //ショットの現在の状態
    
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        state = SlotState.Ready;
        particle = GetComponent<ParticleSystem>();
        particleMain = particle.main;

        if(shot != null){
            shotAmount = shot.shotData.maxAmount;
            shotColor = shot.gameObject.GetComponent<Renderer>().sharedMaterial.color;
            particleMain.startColor = shotColor;
        }
        
    }

    public SlotState GetState()
    {
        return state;
    }

    public void SetState(SlotState newState)
    {
        state = newState;
    }

    public void SetShot(Shot newShot)
    {
        shot = newShot;
        shotAmount = 0;
        StartCoroutine(ShotRechargeCo());
        shotColor = shot.gameObject.GetComponent<Renderer>().sharedMaterial.color; //色をショットのマテリアルから取得
        particleMain.startColor = shotColor; //マテリアルから取得した色をパーティクルの色に設定
    }

    //ショットを放つ
    public void Fire() 
    {   
        if(shot == null) return;
        if(state != SlotState.Ready) return;
        if(shotAmount <= 0){
            StartCoroutine(ShotRechargeCo());
            return;
        }
        
        --shotAmount; 
        particle.Play();
        shot.Instance();

        if(shotAmount > 0){ //ショットを放った後に残弾数が0だったら自動でリチャージ
            StartCoroutine(ShotRateCo());
        }
        else{
            StartCoroutine(ShotRechargeCo());
        }
        
    }   

    //ショットの連射速度をコルーチンで制御
    private IEnumerator ShotRateCo() 
    {
        state = SlotState.Wait;
        yield return new WaitForSeconds(shot.shotData.rate);
        state = SlotState.Ready;
    }

    private IEnumerator ShotRechargeCo() //ショットのリチャージ
    {
        if(player.energy >= shot.shotData.useEnergy){
            state = SlotState.Recharging;
            player.energy -= shot.shotData.useEnergy;

            float rechargeTime = shot.shotData.rechargeTime;
            yield return new WaitForSeconds(rechargeTime);

            shotAmount = shot.shotData.maxAmount;
            state = SlotState.Ready;
        }
    }
}
