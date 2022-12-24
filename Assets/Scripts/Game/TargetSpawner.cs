using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TargetName{
    ALPHA = 0,
    BETA = 1,
    GANMMA = 2
}

public class TargetSpawner : MonoBehaviour
{
    [SerializeField]private GameObject targetObject;
    [SerializeField]private GameObject[] spawnArea;
    [SerializeField]private TargetHpContainer targetHpContainer;
    private TargetName targetName;

    public void Spawn(int count)
    {
        for(int i = 0; i < count; ++i){
            targetName = (TargetName)i;
            spawnArea = GameObject.FindGameObjectsWithTag("TargetSpawnArea");
            int spawnAreaValue = Random.Range(0, spawnArea.Length);
            Vector3 pos = spawnArea[spawnAreaValue].transform.position;
            pos.y += 2.0f;
            GameObject tar = Instantiate(targetObject, pos, Quaternion.identity);
            tar.GetComponentInChildren<TextMeshProUGUI>().text = targetName.ToString();
            targetHpContainer.Placement(tar, targetName.ToString());

            spawnArea[spawnAreaValue].SetActive(false);
        }
        
        foreach(Transform child in transform){
            child.gameObject.SetActive(true);
        }
    }

}
