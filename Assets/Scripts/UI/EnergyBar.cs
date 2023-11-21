using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Energy energy;
    private Slider slider;

    void Start()
    {
        TryGetComponent(out slider);
        slider.maxValue = energy.MaxEnergy;
        slider.value = energy.CurrentEnergy;
        energy.onUpdateEnergy += UpdateSlider;
    }

    void UpdateSlider(float amount)
    {
        slider.value = amount;
    }
}
