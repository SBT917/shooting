using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotAmountBar : MonoBehaviour
{
    private Player player;
    private Slider slider;
    [SerializeField] private ShotSlot slot;
    [SerializeField] private Image rechargeGauge;
    
    void Start()
    {
        player = GetComponent<Player>();
        slider = GetComponent<Slider>();
        rechargeGauge.gameObject.SetActive(false);
    }

    void Update()
    {
        if(slot.shot == null){
            slider.value = 0.0f;
            return;
        }

        slider.maxValue = slot.shot.shotData.maxAmount;
        slider.value = slot.shotAmount;

        if(slot.GetState() == SlotState.Recharging && !rechargeGauge.gameObject.activeSelf){
            StartCoroutine(RechargeGauge());
        }
    }

    private IEnumerator RechargeGauge()
    {
        rechargeGauge.gameObject.SetActive(true);
        float currentTime = slot.shot.shotData.rechargeTime;
        while(slot.GetState() == SlotState.Recharging)  
        {
            currentTime -= Time.deltaTime;
            rechargeGauge.fillAmount = currentTime / slot.shot.shotData.rechargeTime;
            yield return null;
        }
        rechargeGauge.gameObject.SetActive(false);
    }
}
