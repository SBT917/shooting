using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetHpContainer : MonoBehaviour
{
    [SerializeField]private GameObject targetHpBar;

    public void Placement(GameObject target, string targetName)
    {
        GameObject bar = Instantiate(targetHpBar, transform);
        bar.GetComponentInChildren<TargetHpBar>().targetObject = target;
        string text = targetName;
        bar.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    public void BarDestroy()
    {
        foreach(Transform container in gameObject.transform){
            GameObject.Destroy(container.gameObject);
        }
    }
}
