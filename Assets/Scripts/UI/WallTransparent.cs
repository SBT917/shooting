using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//プレイヤーが壁の中に入った時に壁を貫通してプレイヤーを表示するUIを制御する
public class WallTransparent : MonoBehaviour
{
    private Player player;
    private RawImage renderTexture;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();    
        renderTexture = GetComponentInChildren<RawImage>();
    }

    void Update()
    {
        if(player.GetState() == PlayerState.Invisible && player.inWall){
            renderTexture.gameObject.SetActive(true);
        }
        else{
            renderTexture.gameObject.SetActive(false);
        }
    }
}
