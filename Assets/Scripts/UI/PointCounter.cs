using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointCounter : MonoBehaviour
{
    private Player player;
    private int nowPoint;
    private TextMeshProUGUI text;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        nowPoint = player.nowPoint;
        text.text = "x" + nowPoint.ToString("00");
    }
}
