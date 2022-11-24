using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShotSlot : MonoBehaviour
{   
    public GameObject shot;
    private TextMeshProUGUI text;
    private Image image;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
    }

    void Update() 
    {
        if(shot != null){
            text.text = shot.GetComponent<Shot>().shotData.shotName;
            image.color = shot.GetComponent<Shot>().shotData.rarity.color;
        }
        else{
            text.text = "None";
            image.color = Color.grey;
        }
       
    }

    public void ChangeShot(GameObject changeShot)
    {
        shot = changeShot;  
    }

}
