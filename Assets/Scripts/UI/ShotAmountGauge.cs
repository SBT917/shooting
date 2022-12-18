using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//弾の残弾を表すゲージの制御
public class ShotAmountGauge : MonoBehaviour
{
    private Player player; //プレイヤー
    private Image gauge;
    [SerializeField] private ShotSlot slot; //情報を受け取るショットスロット
    [SerializeField] private Image rechageImage;
    

    //[SerializeField] private Image rechargeGauge;
    
    void Start()
    {
        player = GetComponent<Player>();
        gauge = GetComponent<Image>();
    }

    void Update()
    {
        if(slot.shot == null){
            gauge.fillAmount = 0;
            return;
        }
        
        gauge.color = slot.shotColor;
        gauge.fillAmount = (float)slot.shotAmount / slot.shot.shotData.maxAmount; //ゲージとスロットの残弾数の同期

        if(slot.GetState() == SlotState.Recharging && !rechageImage.gameObject.activeSelf){
            StartCoroutine(RechargeGauge());
        }
    }

    private IEnumerator RechargeGauge()
    {
        rechageImage.gameObject.SetActive(true);
        rechageImage.fillAmount = 0;
        float currentTime = slot.shot.shotData.rechargeTime;
        while(slot.GetState() == SlotState.Recharging)  
        {
            currentTime -= Time.deltaTime;
            rechageImage.fillAmount = 1 - currentTime / slot.shot.shotData.rechargeTime;
            yield return null;
        }
        rechageImage.gameObject.SetActive(false);
    }
}
