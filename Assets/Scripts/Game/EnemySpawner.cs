using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//敵をスポーンさせるオブジェクトの制御
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]private GameObject[] spawnEnemys; //スポーンするエネミー達
    [SerializeField]private GameObject[] spawnArea; //スポーンする場所
    [SerializeField]private TextMeshProUGUI enemyCountText; //エネミーの数を示すテキスト
    [SerializeField]private Image spawnGauge; //エネミーがスポーンするまでの時間を表すUI
    [SerializeField]private ParticleSystem spawnParticle; //エネミーがスポーンする前に出現するパーティクル
    private GameManager gameManager;
    public bool isEnemySpawning; //スポーンカウントが残っているかどうか
    public int  enemyCount; //エネミーの数
    
    void Start()
    {
        spawnArea = GameObject.FindGameObjectsWithTag("EnemySpawnArea");
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {   
        CounterUpdate();
    }

    private IEnumerator Spawner(GameObject[] enemys)
    {
        spawnEnemys = enemys;

        int spawnAreaValue = Random.Range(0, spawnArea.Length); //スポーンする場所をランダムで選出
        int spawnEnemyValue = Random.Range(0, spawnEnemys.Length); //スポーンする敵をランダムで選出

        Vector3 areaEdge = Vector3.zero;

        //spawnAreaの四角形の内側の何処かから出現させる
        areaEdge.x = spawnArea[spawnAreaValue].transform.position.x - spawnArea[spawnAreaValue].transform.localScale.x * 5;
        areaEdge.z = spawnArea[spawnAreaValue].transform.position.z + spawnArea[spawnAreaValue].transform.localScale.z * 5;

        Vector3 spawnRange = new Vector3(Random.Range(areaEdge.x, areaEdge.x + (spawnArea[spawnAreaValue].transform.localScale.x * 5) * 2), 0,
                                        Random.Range(areaEdge.z, areaEdge.z - (spawnArea[spawnAreaValue].transform.localScale.z * 5) * 2));
        Instantiate(spawnParticle, spawnRange, Quaternion.identity);
        yield return new WaitForSeconds(spawnParticle.main.duration);
        Instantiate(spawnEnemys[spawnEnemyValue], spawnRange, Quaternion.identity);
    }

    public IEnumerator SpawnCo(GameObject[] enemys, int count, int span, int spawnOneTime)
    {   
        spawnGauge.gameObject.SetActive(true);
        isEnemySpawning = true;
        int spawnCount = count;
        float timeCnt = 0;
        while(true){
            if(timeCnt <= 0){
                timeCnt = span;
                --spawnCount;
                for(int i = 0; i < spawnOneTime; ++i){
                    ++enemyCount;
                    StartCoroutine(Spawner(enemys));
                }

                if(spawnCount <= 0){
                    spawnGauge.gameObject.SetActive(false);
                    isEnemySpawning = false;
                    yield break;
                }
            }
            timeCnt = Mathf.MoveTowards(timeCnt, timeCnt - 1, Time.deltaTime * 1.0f); //ゲージをなめらかに見せるため補完して減らす
            spawnGauge.fillAmount = 1 - (timeCnt / span);
            yield return null;
        } 
    }

    private void CounterUpdate()
    {
        enemyCountText.text = "x" + enemyCount.ToString("00"); //エネミーカウンターのUIを更新
        enemyCountText.gameObject.SetActive(!gameManager.isBossWave); //ボス戦時はカウンターを非表示にする

        if(enemyCount < 0){
            enemyCount = 0;
        }
    }
}
