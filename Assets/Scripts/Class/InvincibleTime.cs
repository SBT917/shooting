using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleTime : MonoBehaviour
{
    [SerializeField]private float effectTime;
    public bool IsInvincible {  get; set; }

    private float currentTime;
    private MeshRenderer mesh;

    void Awake()
    {
        if(!TryGetComponent(out mesh))
        {
            mesh = GetComponentInChildren<MeshRenderer>();
        }
    }

    void Update()
    {
        if (IsInvincible)
        {
            currentTime += Time.deltaTime;
            if(currentTime > effectTime)
            {
                currentTime = 0;
                IsInvincible = false;
            }
        }
    }

    public void StartInvincible()
    {
        if (IsInvincible) return;
        currentTime = 0;
        IsInvincible = true;
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        while(IsInvincible)
        {
            mesh.enabled = false;
            yield return new WaitForSeconds(0.1f);
            mesh.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
