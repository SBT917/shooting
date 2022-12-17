using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//弾の残弾を表すスライダーの制御
public class ShotAmountBar : MonoBehaviour
{
    private Player player; //プレイヤー
    private Slider slider; //スライダー
    [SerializeField] private ShotSlot slot; //情報を受け取るショットスロット
    [SerializeField] private Image rechargeGauge; //リチャージ時間を表すゲージのImage
    
    void Start()
    {
        player = GetComponent<Player>();
        slider = GetComponent<Slider>();
        rechargeGauge.gameObject.SetActive(false);
    }

    void Update()
    {
        if(slot.shot == null){ //スロットにショットが入っていないなら常に0
            slider.value = 0.0f;
            return;
        }

        slider.maxValue = slot.shot.shotData.maxAmount;
        slider.value = slot.shotAmount; //スライダーとスロットの残弾数の同期

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
