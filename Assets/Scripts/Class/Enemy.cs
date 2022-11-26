using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum EnemyState
{
    Normal,
    Attack,
    Search,
    Death
}

public abstract class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    private Rigidbody rb;
    protected NavMeshAgent nav;

    public float hp;
    public GameObject player;
    public GameObject target;
    public GameObject[] targetObjects;

    private GameManager gameManager;
    private EnemyState state;
    private GameObject enemyCanvas;
    private float canvasDisapCnt;
    

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();

        player = GameObject.FindWithTag("Player");
        targetObjects = GameObject.FindGameObjectsWithTag("Target");  
        enemyCanvas = transform.Find("EnemyCanvas").gameObject;
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        state = EnemyState.Normal;
        hp = enemyData.maxHp;
        nav.speed = enemyData.normalSpeed;
    }

    protected virtual void Update()
    {
        if(GetState() == EnemyState.Death) return;
        Act();
    }

    public void SetState(EnemyState newState)
    {
        state = newState;
    }

    public EnemyState GetState()
    {
        return state;
    }

    public PlayerState GetPlayerState()
    {
        PlayerState playerState = player.GetComponent<Player>().GetState();
        return playerState;
    }

    protected virtual void Act()
    {
        NormalAction();
        AttackAction(enemyData.attackOutRange, enemyData.searchTime);
        nav.SetDestination(target.transform.position);
    }

    protected virtual void Dead()
    {   
        player.GetComponent<Player>().nowScore += enemyData.score;
        ItemDrop();
        gameObject.SetActive(false);
    }

    protected virtual void Disappearing()
    {
        gameObject.SetActive(false);
    }

    protected virtual void NormalAction()
    {
        if(state == EnemyState.Normal)
        {
            nav.speed = enemyData.normalSpeed;

            float[] distances = new float[targetObjects.Length];
            for(int i = 0; i < targetObjects.Length; ++i)
            {
                distances[i] = Vector3.Distance(targetObjects[i].transform.position, transform.position);
            }
            float min = distances.Min();
            int minIndex = Array.IndexOf(distances, min);
            target = targetObjects[minIndex];
        }
    }

    protected virtual void AttackAction(float outRange, float searchTime)
    {
        if(state == EnemyState.Attack)
        {
            nav.speed = enemyData.attackSpeed;
            target = player;

            if(CheckDistance(player) > outRange || GetPlayerState() == PlayerState.Invisible)
            {
                SetState(EnemyState.Search);
                StartCoroutine(SearchCo(searchTime));
            }  
        }
    }
    private IEnumerator SearchCo(float time)
    {
        float count = time * 10;
        nav.speed = 0.0f;
        while(state == EnemyState.Search)
        {   
            yield return new WaitForSeconds(0.1f);
            count--;
            if(count < 0)
            {
                SetState(EnemyState.Normal);
                yield break;
            }
        }
    }

    public float CheckDistance(GameObject target)
    {
        Vector3 yPos = target.transform.position;
        Vector3 mPos = this.transform.position;

        return Vector3.Distance(yPos, mPos);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        SetState(EnemyState.Attack);
        StartCoroutine(CanvasAvtiveCo());

        if(hp <= 0)
        {
            Dead();
        }
    }

    private void ItemDrop()
    {   
        int randValue = UnityEngine.Random.Range(0, 100);
        if(enemyData.itemDropRatio < randValue) return;
        int itemValue = UnityEngine.Random.Range(0, enemyData.dropItems.Length);
        Instantiate(enemyData.dropItems[itemValue], new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.identity);
    }

    private IEnumerator CanvasAvtiveCo()
    {
        canvasDisapCnt = 10.0f;

        if(!enemyCanvas.activeSelf)
        {
            enemyCanvas.SetActive(true);

            while(canvasDisapCnt > 0.0f)
            {
                canvasDisapCnt -= 1.0f;
                yield return new WaitForSeconds(1.0f);
            }
            
            enemyCanvas.SetActive(false);
        }
        else
        {
            yield break;
        }     
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Player":
                other.GetComponent<Player>().TakeDamage(enemyData.attackToPlayer);
                break;
            case "Target":
                other.GetComponent<TargetObject>().TakeDamage(enemyData.attackToObject);
                Disappearing();
                break;
        }
        
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")){
            other.GetComponent<Player>().TakeDamage(enemyData.attackToPlayer);
        }
    }
}
