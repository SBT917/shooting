using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//敵をスポーンさせるオブジェクトの制御
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]private GameObject[] enemys; //スポーンする敵
    [SerializeField]private GameObject[] spawnArea; //スポーンする場所
    [SerializeField]private Image spawnGauge; //エネミーがスポーンするまでの時間を表すUI
    private GameManager gameManager;
    
    void Start()
    {
        spawnArea = GameObject.FindGameObjectsWithTag("EnemySpawnArea");
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {   

    }

    private void Spawner()
    {
        int spawnAreaValue = Random.Range(0, spawnArea.Length); //スポーンする場所をランダムで選出
        int spawnEnemyValue = Random.Range(0, enemys.Length); //スポーンする敵をランダムで選出

        Vector3 areaEdge = Vector3.zero;

        //spawnAreaの四角形の内側の何処かから出現させる
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
                    ++gameManager.enemyCount;
                }
            }
            count = Mathf.MoveTowards(count, count - 1, Time.deltaTime * 10.0f); //ゲージをなめらかに見せるため補完して減らす
            spawnGauge.fillAmount = 1 - (count / span);
            yield return null;
        } 
    }
}
