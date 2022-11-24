using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public enum GameState
{
    Start,
    Game,
    BreakTime,
    GameOver
}

public class GameManager : MonoBehaviour
{
    

    [SerializeField]private GameState state;
    [SerializeField]private TextMeshProUGUI gameText;
    [SerializeField]private TextMeshProUGUI waveText;
    [SerializeField]private TextMeshProUGUI leftTimeText;
    [SerializeField]private TargetHpContainer targetHpContainer;
    [SerializeField]private ScoreCounter scoreCounter;
    [SerializeField]private ShotShop shotShop;
    private EnemySpawner enemySpawner;
    private TargetSpawner targetSpawner;
    private Player player;
    private GameObject[] targetObjects;
    private int waveCount = 1;
    private int startCount;

    private Coroutine spawnCo;
    private Coroutine timeCo;
    private Coroutine scoreCo;

    [SerializeField]private int leftTime;
    [SerializeField]private int enemySpawnSpan;
    [SerializeField]private int enemySpawnOneTime;

    void Awake()
    {
        targetObjects = System.Array.Empty<GameObject>();
        enemySpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        targetSpawner = GameObject.FindWithTag("TargetSpawner").GetComponent<TargetSpawner>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        StartCoroutine(GameStart());
    }

    void Update()
    {   
        TimeSpan timeSpan = new TimeSpan(0, 0, leftTime);
        leftTimeText.text = timeSpan.ToString(@"mm\:ss");
        waveText.text = "WAVE " + waveCount.ToString();

        if(leftTime <= 0){
            StartCoroutine(WaveBetween());
        }
        
        if(CheckPlayerAndTargetState()){
            StartCoroutine(GameOver());
        }
    }

    private IEnumerator GameStart()
    {
        state = GameState.Start;
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
        StopCoroutine(spawnCo);
        EnemyDestroyer();
        shotShop.DrawingShop();
        ++waveCount;
        
        leftTime = 90;
        enemySpawnSpan = 30;
        enemySpawnOneTime = 10;

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
    }

    public void ForceGameStart()
    {
        startCount = 0;
    }

    private IEnumerator GameOver()
    {
        state = GameState.GameOver;
        StopCoroutine(timeCo);
        StopCoroutine(spawnCo);   

        gameText.text = "GAME OVER";
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("TitleScene");
    }

    public GameState GetState()
    {
        return state;
    }

    private IEnumerator TimeCount()
    {
        while(true){
            yield return new WaitForSeconds(1.0f);
            --leftTime;
        }
    }

    private bool CheckPlayerAndTargetState()
    {
        bool isTargetBreak = false;

        for(int i = 0; i < targetObjects.Length; ++i){
            TargetObject target = targetObjects[i].GetComponent<TargetObject>();
            
            if(target.GetState() == TargetObject.TargetState.Break){
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
            Destroy(enemy);
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
}
