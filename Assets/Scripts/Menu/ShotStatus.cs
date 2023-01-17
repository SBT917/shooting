using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//ShotObjectやShotInfoにカーソルが重なった時にショットのステータスを表示する
public class ShotStatus : MonoBehaviour
{
    [SerializeField]private Vector3 panelPosOffset;

    [SerializeField]private GameObject panel;

    [SerializeField]private Slider damageSlider;
    [SerializeField]private Slider rateSlider;
    [SerializeField]private Slider lengthSlider;
    [SerializeField]private Slider amountSlider;
    [SerializeField]private Slider energySlider;
    [SerializeField]private Slider rechargeSlider;


    private GameObject preFrameHitObject = null;
    private List<RaycastResult> results = new List<RaycastResult>();

    private bool isVisible;

    void Update()
    {
        results.Clear();

        var pointData = new PointerEventData(EventSystem.current);
        pointData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(pointData, results);

        
        if(preFrameHitObject == null){
            foreach(RaycastResult result in results){
                var sendShotData = result.gameObject.GetComponent<ISendShotData>();
                if(sendShotData != null){
                    var shotData = sendShotData.SendShotData();
                    if(shotData == null) return;
                    preFrameHitObject = result.gameObject;
                    SetStatus(shotData);
                    isVisible = true;
                    break;
                }
            }
        }
        else{
            if(results.All(ray => ray.gameObject != preFrameHitObject)){
                isVisible = false;
                preFrameHitObject = null;
            }   
            
        }
        
        if(isVisible){
            panel.transform.position = Input.mousePosition + panelPosOffset;
        }

        panel.SetActive(isVisible);
    }

    private void SetStatus(ShotData data)
    {
        damageSlider.value = data.damage;
        rateSlider.value = rateSlider.maxValue - data.rate;
        lengthSlider.value = data.moveSpeed * data.disapCnt;
        amountSlider.value = data.maxAmount;
        energySlider.value = energySlider.maxValue - data.useEnergy;
        rechargeSlider.value = rechargeSlider.maxValue - data.rechargeTime;
    }
}
