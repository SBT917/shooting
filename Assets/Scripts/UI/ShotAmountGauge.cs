using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//弾の残弾を表すゲージの制御
public class ShotAmountGauge : MonoBehaviour
{
    private Image gauge;
    [SerializeField] private ShotLauncher launcher; //情報を受け取るショットランチャー
    [SerializeField] private ShotSlot slot; //情報を受け取るショットスロット
    [SerializeField] private Image rechageImage;
    
    
    void Start()
    {
        TryGetComponent(out gauge);

        gauge.fillAmount = launcher.MaxAmount;
        gauge.color = launcher.ShotColor;

        launcher.onLaunch += UpdateGauge;
        launcher.onUpdateRecharge += UpdateRechargeGauge;
        launcher.onFinishRecharge += UpdateGauge;
    }

    private void UpdateGauge(int amount)
    {
        gauge.fillAmount = (float)amount / launcher.MaxAmount;
    }

    private void UpdateRechargeGauge(float count)
    {
        if(!rechageImage.gameObject.activeSelf) rechageImage.gameObject.SetActive(true);
        rechageImage.fillAmount = 1 - count / slot.shot.ShotData.rechargeTime;
        if (launcher.RechargeCount <= 0) rechageImage.gameObject.SetActive(false);
    }

}
