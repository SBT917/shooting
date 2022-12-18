using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum TargetState{
        Normal,
        Break
    }

public class TargetObject : MonoBehaviour
{   
    private GameManager gm;
    public float maxHp = 100.0f;
    public float hp;
    private TargetState state;
    [SerializeField]private CinemachineVirtualCamera cinemachine;
    [SerializeField]private ParticleSystem hitParticle;
    [SerializeField]private ParticleSystem breakParticle;

    void Start()
    {
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        cinemachine = GameObject.FindWithTag("CinemachineVirtualCamera").GetComponent<CinemachineVirtualCamera>();
        hp = maxHp;
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if(gm.GetState() != GameState.Game) return;
        hp -= damage;
        hitParticle.Play();
        if(hp <= 0)
        {
            state = TargetState.Break;
            cinemachine.Follow = transform;
            ParticleSystem p = Instantiate<ParticleSystem>(breakParticle, transform.position, Quaternion.identity);
            p.Play();
            gameObject.SetActive(false);
        }
    }

    protected virtual void Destroy()
    {
        gameObject.SetActive(false);
    }

    public TargetState GetState()
    {
        return state;
    }
}
