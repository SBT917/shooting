using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{   
    private Player player;
    private int nowScore;
    private TextMeshProUGUI text;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {   
        text.text = nowScore.ToString("D7"); 
    }

    public IEnumerator AppendScore()
    {
        while(true){
            if(nowScore != player.nowScore){
                ++nowScore;
            }
            yield return null;
        }    
    }   
}   
