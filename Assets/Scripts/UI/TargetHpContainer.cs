using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetHpContainer : MonoBehaviour
{
    [SerializeField]private GameObject targetHpBar;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Placement(GameObject target, int num)
    {
        GameObject bar = Instantiate(targetHpBar, transform);
        bar.GetComponentInChildren<TargetHpBar>().targetObject = target;
        string text = "TARGET" + num.ToString();
        bar.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    public void BarDestroy()
    {
        foreach(Transform container in gameObject.transform){
            GameObject.Destroy(container.gameObject);
        }
    }
}
