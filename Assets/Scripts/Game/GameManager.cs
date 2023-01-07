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
    [SerializeField]private TargetHpContainer targetHpContainer;
    [SerializeField]private ScoreCounter scoreCounter;
    [SerializeField]private ShotShop shotShop;
    [SerializeField]private EnemySpawnGroup[] enemySpawnGroups;
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
        startCount = 2;
        
        while(startCount > 0){
            gameText.text = startCount.ToString();
            if(startCount == 1){
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
        audioManager.PlayCheck();

        shotShop.DrawingShop();
        ChangeLevel();

        if(isBossWave) gameText.color = Color.red;

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
        
        audioManager.PlayCheck();
        targetObjects = GameObject.FindGameObjectsWithTag("Target");
        gameText.text = "";
    
        spawnCo = StartCoroutine(enemySpawner.SpawnCo(enemySpawnGroups[enemySpawnGroupIndex].enemys, enemySpawnCount, enemySpawnSpan, enemySpawnOneTime));
    }

    public void BossDestroying()
    {
        isBossWave = false;
        StartCoroutine(HitStop());

        gameText.color = Color.white;
        EnemyDestroyer();
        StartCoroutine(TargetRelocation(2));
        StartCoroutine(WaveBetween());
    }

    public void ForceGameStart()
    {
        startCount = 0;
    }

    private IEnumerator GameOver()
    {
        state = GameState.GameOver;
        
        audioManager.PlayCheck();
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
        foreach(GameObject target in targetObjects){
            Destroy(target);
        }
        targetHpContainer.BarDestroy();
        targetObjects = System.Array.Empty<GameObject>();

        yield return new WaitForSeconds(5.0f);

        targetSpawner.Spawn(count);
        targetObjects = GameObject.FindGameObjectsWithTag("Target");
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
        if(waveCount == 1){
            SetParameter(1, 0, 0, 1);
        }
        if(waveCount >= 2 && waveCount < 3){
            isBossWave = true;
            SetParameter(0, 0, 0, 1);
        }
        else if(waveCount >= 3 && waveCount < 6){
            SetParameter(1, 3, 20, 15);
        }
        else if(waveCount >= 6 && waveCount < 10){
            SetParameter(2, 6, 10, 15);
        }
        else if(waveCount >= 10){
            SetParameter(3, 6, 10, 20);
        }
    }
}
