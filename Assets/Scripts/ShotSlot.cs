using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotState
{
    Ready,
    Wait,
    Recharging
}

public class ShotSlot : MonoBehaviour
{
    public Shot shot;
    public int shotAmount;
    private Player player;
    private Color shotColor;
    private ParticleSystem particle;
    private ParticleSystem.MainModule particleMain;
    [SerializeField]private SlotState state;
    
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
        shotAmount = newShot.shotData.maxAmount;
        shotColor = shot.gameObject.GetComponent<Renderer>().sharedMaterial.color;
        particleMain.startColor = shotColor;
    }

    public void Fire()
    {   
        if(state != SlotState.Ready) return;
        if(shot == null) return;
        
        particle.Play();
        shot.Instance();
        --shotAmount;
        if(shotAmount <= 0){
            StartCoroutine(ShotRecharge(0, shot));
            return;
        }
        float shotRate = shot.shotData.rate;
        StartCoroutine(ShotRateCo(shotRate));
    }

    private IEnumerator ShotRateCo(float rate)
    {
        state = SlotState.Wait;
        yield return new WaitForSeconds(rate);
        state = SlotState.Ready;
    }

    private IEnumerator ShotRecharge(int slotNum, Shot shot)
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
