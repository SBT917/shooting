using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShotInfo : MonoBehaviour
{   
    public Shot shot;
    public ShotSlot slot;
    private TextMeshProUGUI text;
    private Image image;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
    }

    void Update() 
    {
        shot = slot.shot;

        if(shot != null){
            text.text = shot.shotData.shotName;
            image.color = shot.shotData.rarity.color;
        }
        else{
            text.text = "None";
            image.color = Color.grey;
        }
       
    }

}
