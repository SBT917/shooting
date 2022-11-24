using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetHpBar : MonoBehaviour
{
    public GameObject targetObject;
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = targetObject.GetComponent<TargetObject>().maxHp;
    }

    void Update()
    {
        slider.value = targetObject.GetComponent<TargetObject>().hp;
    }
}
