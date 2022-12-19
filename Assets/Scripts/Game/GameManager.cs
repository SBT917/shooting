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
    [SerializeField]private TextMeshProUGUI enemyCountText;
    [SerializeField]private TargetHpContainer targetHpContainer;
    [SerializeField]private ScoreCounter scoreCounter;
    [SerializeField]private ShotShop shotShop;
    public int enemyCount;
    private bool isEnemySpawning;
    private EnemySpawner enemySpawner;
    private TargetSpawner targetSpawner;
    private Player player;
    private GameObject[] targetObjects;
    private int waveCount;
    private int startCount;

    private Coroutine spawnCo;
    private Coroutine timeCo;
    private Coroutine scoreCo;

    public static int finalScore;
    public static int finalWave;

    [SerializeField]private int enemySpawnTime;
    [SerializeField]private int enemySpawnSpan;
    [SerializeField]private int enemySpawnOneTime;

    void Awake()
    {
        Time.timeScale = 1.0f;
        targetObjects = System.Array.Empty<GameObject>();
        enemySpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        targetSpawner = GameObject.FindWithTag("TargetSpawner").GetComponent<TargetSpawner>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        StartCoroutine(GameStart());
    }

    void Update()
    {   
        enemyCountText.text = "x" + enemyCount.ToString("00");
        waveText.text = "WAVE " + waveCount.ToString();

        if(enemySpawnTime <= 0 && isEnemySpawning){
            StopCoroutine(spawnCo);
            isEnemySpawning = false;
        }

        if(enemyCount <= 0 && !isEnemySpawning && state == GameState.Game){
            StartCoroutine(WaveBetween());
        }
        
        if(CheckPlayerAndTargetState()){
            StartCoroutine(GameOver());
        }
    }

    private IEnumerator GameStart()
    {
        state = GameState.Start;
        SetLevel();
        startCount = 10;
        
        while(startCount > 0){
            gameText.text = startCount.ToString();
            if(startCount == 5){
                targetSpawner.Spawner(1);
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
        if(waveCount % 5 == 0){
            StartCoroutine(TargetRelocation(2));
        }

        StopCoroutine(timeCo);
        EnemyDestroyer();
        shotShop.DrawingShop();
        SetLevel();

        startCount = 30;
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
        targetObjects = GameObject.FindGameObjectsWithTag("Target");
        gameText.text = "";
        timeCo = StartCoroutine(TimeCount());
        spawnCo = StartCoroutine(enemySpawner.SpawnCo(enemySpawnSpan, enemySpawnOneTime));
        isEnemySpawning = true;
    }

    public void ForceGameStart()
    {
        startCount = 0;
    }

    private IEnumerator GameOver()
    {
        state = GameState.GameOver;

        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(2.0f);
        Time.timeScale = 1.0f;

        StopCoroutine(timeCo);
        
        finalWave = waveCount;
        finalScore = player.nowScore;

        yield return new WaitForSecondsRealtime(3.0f);
        SceneManager.LoadScene("ResultScene");
    }

    public GameState GetState()
    {
        return state;
    }

    private IEnumerator TimeCount()
    {
        while(true){
            yield return new WaitForSeconds(1.0f);
            --enemySpawnTime;
        }
    }

    private bool CheckPlayerAndTargetState()
    {
        bool isTargetBreak = false;

        for(int i = 0; i < targetObjects.Length; ++i){
            TargetObject target = targetObjects[i].GetComponent<TargetObject>();
            
            if(target.GetState() == TargetState.Break){
                isTargetBreak = true;
                break;
            }
        }

        return player.GetState() == PlayerState.Death || isTargetBreak;
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
        foreach(GameObject target in targetObjects){
            Destroy(target);
        }
        targetHpContainer.BarDestroy();
        targetObjects = System.Array.Empty<GameObject>();

        yield return new WaitForSeconds(5.0f);

        targetSpawner.Spawner(count);
        targetObjects = GameObject.FindGameObjectsWithTag("Target");
    }

    private void SetParameter(int time, int spawnSpan, int spawnOneTime)
    {
        enemySpawnTime = time;
        enemySpawnSpan = spawnSpan;
        enemySpawnOneTime = spawnOneTime;
    }

    private void SetLevel()
    {
        ++waveCount;
        if(waveCount >= 1 && waveCount < 3){
            SetParameter(60, 100, 10);
        }
        else if(waveCount >= 3 && waveCount < 6){
            SetParameter(60, 100, 15);
        }
        else if(waveCount >= 6 && waveCount < 10){
            SetParameter(90, 50, 15);
        }
        else if(waveCount >= 10){
            SetParameter(90, 50, 20);
        }
    }
}
