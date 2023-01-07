using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//メニュー内の現在のショットを表示させるUIを制御するクラス
public class ShotInfo : MonoBehaviour
{   
    public Shot shot; //現在のショット
    public ShotSlot slot; //対応するスロット
    private TextMeshProUGUI nameText; //ショット名を表示させるテキスト
    [SerializeField]private Image Frame; //フレーム(ショットのレアリティによって色を変える)
    [SerializeField]private Image icon; //ショットのアイコンを表示させるアイコン

    void Start()
    {
        nameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update() 
    {
        shot = slot.shot;

        if(shot != null){
            nameText.text = shot.shotData.shotName;
            icon.sprite = shot.shotData.icon;
            Frame.color = shot.shotData.rarity.color;
        }
        else{
            nameText.text = "-";
            Frame.color = Color.white;
        }
       
    }

}
