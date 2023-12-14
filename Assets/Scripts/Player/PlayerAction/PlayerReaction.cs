using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReaction : MonoBehaviour, IDamageable
{
    private Hp hp;

    // Start is called before the first frame update
    void Awake()
    {
        TryGetComponent(out hp);
    }

    public void TakeDamage(float damage)
    {
        hp.DecreaseHp(damage);
    }

}
