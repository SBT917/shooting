using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointBonus : MonoBehaviour
{
    private Player player;
    private TextMeshProUGUI text;

    [SerializeField]private int bonusBaseValue;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        text = GetComponent<TextMeshProUGUI>();
        text.enabled = false;
    }

    public void AppendBonus()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        
        float totalMaxHp = 0;
        float totalHp = 0;

        foreach(GameObject g in targets){
            TargetObject target = g.GetComponent<TargetObject>();
            totalMaxHp += target.maxHp;
            totalHp += target.hp;
        }

        int bonus = (int)(bonusBaseValue * (totalHp / totalMaxHp)) * targets.Length;
        player.nowPoint += bonus;
        
        StartCoroutine(DisplayText(bonus));   
    }

    private IEnumerator DisplayText(int bonus)
    {
        if(bonus == 0) yield break; 
        text.text = "BONUS +" + bonus;
        text.color = new Color32(255, 255, 255, 255);
        text.enabled = true;
        yield return new WaitForSeconds(3.0f);
        for(int i = 0; i < 255; ++i){
            text.color -= new Color32(0, 0, 0, 2);
            yield return null;
        }
        text.enabled = false;
    }
}
