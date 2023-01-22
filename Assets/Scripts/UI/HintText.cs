using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintText : MonoBehaviour
{
    [SerializeField]private GameObject menu;
    private TextMeshProUGUI text;
    private bool isOpenedMenu;

    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if(!menu.activeSelf){
            text.text = "Press E to open the menu";
        }
        else{
            text.text = "Press E to close the menu";
            isOpenedMenu = true;
            return;
        }

        if(isOpenedMenu){
            text.text = "Press G to skip.";
        }
    }
}
