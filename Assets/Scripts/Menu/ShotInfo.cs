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
    private Image backImage; //背景のイメージ(ショットのレアリティによって色を変える時に使用する)
    [SerializeField]private Image icon; //ショットのアイコンを表示させるアイコン

    void Start()
    {
        nameText = GetComponentInChildren<TextMeshProUGUI>();
        backImage = GetComponent<Image>();
    }

    void Update() 
    {
        shot = slot.shot;

        if(shot != null){
            nameText.text = shot.shotData.shotName;
            icon.sprite = shot.shotData.icon;
            backImage.color = shot.shotData.rarity.color;
        }
        else{
            nameText.text = "-";
            backImage.color = Color.grey;
        }
       
    }

}
