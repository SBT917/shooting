using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]private GameObject[] enemys;
    [SerializeField]private GameObject[] spawnArea;
    
    void Start()
    {
        spawnArea = GameObject.FindGameObjectsWithTag("EnemySpawnArea");
    }

    void Update()
    {   

    }

    private void Spawner()
    {
        int spawnAreaValue = Random.Range(0, spawnArea.Length);
        int spawnEnemyValue = Random.Range(0, enemys.Length);

        Vector3 areaEdge = Vector3.zero;
        areaEdge.x = spawnArea[spawnAreaValue].transform.position.x - spawnArea[spawnAreaValue].transform.localScale.x * 5;
        areaEdge.z = spawnArea[spawnAreaValue].transform.position.z + spawnArea[spawnAreaValue].transform.localScale.z * 5;

        Vector3 spawnRange = new Vector3(Random.Range(areaEdge.x, areaEdge.x + (spawnArea[spawnAreaValue].transform.localScale.x * 5) * 2), 0,
                                        Random.Range(areaEdge.z, areaEdge.z - (spawnArea[spawnAreaValue].transform.localScale.z * 5) * 2));
        Instantiate(enemys[spawnEnemyValue], spawnRange, Quaternion.identity);
    }

    public IEnumerator SpawnCo(int span, int spawnOneTime)
    {   
        float count = 0;
        while(true){
            if(count <= 0){
                count = span;
                for(int i = 0; i < spawnOneTime; ++i){
                    Spawner();
                }
            }
            --count;   
            yield return new WaitForSeconds(1.0f);
        } 
    }
}
