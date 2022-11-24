using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{   
    public enum TargetState{
        Normal,
        Break
    }

    public float maxHp = 100.0f;
    public float hp;
    private TargetState state;
    
    void Start()
    {
        hp = maxHp;
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            state = TargetState.Break;
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
