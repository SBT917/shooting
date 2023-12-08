using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//EnergyBarの横に表示されているアイコンの制御
public class GhostIcon : MonoBehaviour
{
    [SerializeField]PlayerInvisible invisible; //透明化の情報を受け取る
    [SerializeField]private Sprite normalSprite; //通常時のSprite
    [SerializeField]private Sprite invisibleSprite; //透明化時のSprite
    private Image icon;

    void Start()
    {
        TryGetComponent(out icon);

        invisible.onStartInvisible += () => icon.sprite = invisibleSprite;
        invisible.onEndInvisible += () => icon.sprite = normalSprite;
    }


}
