using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReaction : MonoBehaviour, IDamageable
{
    private Hp hp;
    private InvincibleTime invincibleTime;

    // Start is called before the first frame update
    void Awake()
    {
        TryGetComponent(out hp);
        TryGetComponent(out invincibleTime);
    }

    public void TakeDamage(float damage)
    {
        if (invincibleTime.IsInvincible) return;

        hp.DecreaseHp(damage);
        invincibleTime.StartInvincible();
    }

}
