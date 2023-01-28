using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

//現在のゲームの状態
public enum GameState
{
    Start, //開始直後
    Game, //ゲーム中
    BreakTime, //Waveの間
    GameOver //ゲームオーバー
}


//ゲーム進行を制御する
public class GameManager : MonoBehaviour
{
    [SerializeField]private GameState state;
    [SerializeField]private TextMeshProUGUI gameText;
    [SerializeField]private TextMeshProUGUI waveText;
    [SerializeField]private TextMeshProUGUI hintText;
    [SerializeField]private GameObject menu;
    [SerializeField]private TargetHpContainer targetHpContainer;
    [SerializeField]private ScoreCounter scoreCounter;
    [SerializeField]private ShotShop shotShop;
    [SerializeField]private EnemySpawnGroup[] enemySpawnGroups;
    [SerializeField]private PointBonus pointBonus;
    private AudioManager audioManager;
    private EnemySpawner enemySpawner;
    private TargetSpawner targetSpawner;
    private Player player;
    private GameObject[] targetObjects;
    private int waveCount;
    private int startCount;

    private Coroutine spawnCo;
    private Coroutine scoreCo;

    public bool isBossWave;
    public static int finalScore;
    public static int finalWave;

    [SerializeField]private int enemySpawnGroupIndex;
    [SerializeField]private int enemySpawnCount;
    [SerializeField]private int enemySpawnSpan;
    [SerializeField]private int enemySpawnOneTime;

    void Awake()
    {
        Time.timeScale = 1.0f;
        targetObjects = System.Array.Empty<GameObject>();
        enemySpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        targetSpawner = GameObject.FindWithTag("TargetSpawner").GetComponent<TargetSpawner>();
        audioManager = GetComponentInChildren<AudioManager>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        StartCoroutine(GameStart());
    }

    void Update()
    {   
        waveText.text = "WAVE " + waveCount.ToString();

        CheckEnemyCount();
        CheckGameOver();
    }

    private IEnumerator GameStart()
    {
        state = GameState.Start;
        ChangeLevel();
        
        startCount = 10;
        
        while(startCount > 0){
            gameText.text = startCount.ToString();
            if(startCount == 5){
                targetSpawner.Spawn(1);
            }
            yield return new WaitForSeconds(1.0f);
            --startCount;
        }
        scoreCo = StartCoroutine(scoreCounter.AppendScore());
        WaveStart();
    }

    private IEnumerator WaveBetween()
    {   
        state = GameState.BreakTime;
        audioManager.PlayBGM();
        pointBonus.AppendBonus();
        hintText.gameObject.SetActive(true);
        
        shotShop.DrawingShop();
        ChangeLevel();

        startCount = 30;
        if(isBossWave) gameText.color = Color.red;
        while(startCount > 0){
            gameText.text = startCount.ToString();
            yield return new WaitForSeconds(1.0f);
            --startCount;
        }

        WaveStart();
    }

    private void WaveStart()
    {
        state = GameState.Game;

        audioManager.PlayBGM();
        targetObjects = GameObject.FindGameObjectsWithTag("Target");
        gameText.text = "";
        hintText.gameObject.SetActive(false);
    
        spawnCo = StartCoroutine(enemySpawner.SpawnCo(enemySpawnGroups[enemySpawnGroupIndex].enemys, enemySpawnCount, enemySpawnSpan, enemySpawnOneTime));
    }

    public void BossDestroying()
    {
        isBossWave = false;
        StartCoroutine(HitStop());
        
        gameText.color = Color.white;
        EnemyDestroyer();
        StartCoroutine(WaveBetween());
    }

    public void ForceGameStart()
    {
        if(targetObjects.Length == 0) return;
        if(menu.activeSelf) return;
        startCount = 0;
    }

    private IEnumerator GameOver()
    {
        state = GameState.GameOver;
        
        audioManager.PlayBGM();
        StartCoroutine(HitStop());
        
        finalWave = waveCount;
        finalScore = player.nowScore;

        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("ResultScene");
    }

    public GameState GetState()
    {
        return state;
    }

    private void CheckEnemyCount()
    {
        if(isBossWave) return;
        if(enemySpawner.isEnemySpawning) return;
        if(enemySpawner.enemyCount > 0) return;
        if(state != GameState.Game) return;

        StartCoroutine(WaveBetween());
    }

    private void CheckGameOver()
    {
        if(state == GameState.GameOver) return;
        bool isTargetBreak = false;

        for(int i = 0; i < targetObjects.Length; ++i){
            TargetObject target = targetObjects[i].GetComponent<TargetObject>();
            
            if(target.GetState() == TargetState.Break){
                isTargetBreak = true;
                break;
            }
        }

        if(player.GetState() == PlayerState.Death || isTargetBreak){
            StartCoroutine(GameOver());
        }
    }

    private void EnemyDestroyer()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemys){
            enemy.GetComponent<Enemy>().Disappearing();
        }
    }

    private IEnumerator TargetRelocation(int count)
    {
        hintText.gameObject.SetActive(false);

        foreach(GameObject target in targetObjects){
            Destroy(target);
        }
        
        targetHpContainer.BarDestroy();
        targetObjects = System.Array.Empty<GameObject>();

        yield return new WaitForSeconds(5.0f);

        targetSpawner.Spawn(count);
        targetObjects = GameObject.FindGameObjectsWithTag("Target");

        hintText.gameObject.SetActive(true);
    }

    private IEnumerator HitStop()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(2.0f);
        Time.timeScale = 1.0f;
    }

    //spawnCount:敵がスポーンする回数
    //spawnSpan:敵がスポーンする間隔
    //spawnOneTime:敵が一度にスポーンする量
    private void SetParameter(int spawnGroup, int spawnCount, int spawnSpan, int spawnOneTime)
    {
        enemySpawnGroupIndex = spawnGroup;
        enemySpawnCount = spawnCount;
        enemySpawnSpan = spawnSpan;
        enemySpawnOneTime = spawnOneTime;
    }

    private void ChangeLevel()
    {
        ++waveCount;
        isBossWave = waveCount % 5 == 0;
        int index = waveCount % 5;
        
        if(!isBossWave){
            int count = Mathf.Clamp(3 + ((waveCount - 1) * 2), 0, 10);
            int span = Mathf.Clamp(30 - ((waveCount - 1) * 5), 15, 30);
            int oneTime = Mathf.Clamp(10 + ((waveCount - 1) * 2), 0, 30);
            SetParameter(index, count, span, oneTime);
            //SetParameter(1, 1, 1, 1);

            if((waveCount - 1) % 5 == 0 && waveCount != 1){
                int targetCount = Mathf.Clamp(((waveCount - 1) / 5) + 1, 1, 4);
                StartCoroutine(TargetRelocation(targetCount));
            }
        }
        else{
            SetParameter(0, 1, 0, 1);
        }

    }
}
