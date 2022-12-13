using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField]private GameObject targetObject;
    [SerializeField]private GameObject[] spawnArea;
    [SerializeField]private TargetHpContainer targetHpContainer;

    public void Spawner(int count)
    {
        for(int i = 0; i < count; ++i){
            spawnArea = GameObject.FindGameObjectsWithTag("TargetSpawnArea");
            int spawnAreaValue = Random.Range(0, spawnArea.Length);
            Vector3 pos = spawnArea[spawnAreaValue].transform.position;
            pos.y += 2.0f;
            GameObject tar = Instantiate(targetObject, pos, Quaternion.identity);
            targetHpContainer.Placement(tar, i + 1);

            spawnArea[spawnAreaValue].SetActive(false);
        }
        
        foreach(Transform child in transform){
            child.gameObject.SetActive(true);
        }
    }

}
