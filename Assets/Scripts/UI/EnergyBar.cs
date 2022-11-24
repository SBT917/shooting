using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    private GameObject player;
    private Slider slider;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        slider.maxValue = player.GetComponent<Player>().maxEnergy;
        slider.value = player.GetComponent<Player>().energy;
    }
}
