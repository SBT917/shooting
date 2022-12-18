using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//EnergyBarの横に表示されているアイコンの制御
public class GhostIcon : MonoBehaviour
{
    private Player player;
    [SerializeField]private Sprite normalSprite; //通常時のSprite
    [SerializeField]private Sprite invisibleSprite; //透明化時のSprite
    private Image icon;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        icon = GetComponent<Image>();
    }

    void Update()
    {
        if(player.GetState() == PlayerState.Invisible){
            icon.sprite = invisibleSprite;
        }
        else{
            icon.sprite = normalSprite;
        }
    }
}
