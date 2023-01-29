using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleEnemy : Enemy
{
    [SerializeField]private MeshRenderer[] meshs;
    private bool isInvisible;

    protected override void Awake()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mesh in meshs){
            foreach(Material material in mesh.materials){
                StartCoroutine(FadeOut(material));
            }
        }
        isInvisible = true;
        base.Awake();
    }
    
    protected override void Update()
    {
        base.Update();

        if(gameManager.GetState() == GameState.GameOver) return;
        if(state == EnemyState.Death) return;
        
        if(Vector3.Distance(transform.position, target.transform.position) < 5.0f || state != EnemyState.Normal){
            Visualize();
        }
    }

    protected override void AttackAction(float outRange, float searchTime)
    {
        if(isInvisible) return;

        base.AttackAction(outRange, searchTime);
    }

    private void Visualize()
    {
        if(isInvisible == false) return;
        isInvisible = false;
        foreach(MeshRenderer mesh in meshs){
            foreach(Material material in mesh.materials){
                StartCoroutine(FadeIn(material));
            }
        } 
    }

    private IEnumerator FadeIn(Material material)
    {
        for(int i = 0; i < 255; ++i){
            material.color += new Color32(0, 0, 0, 1);
            yield return null;
        }
    }

    private IEnumerator FadeOut(Material material)
    {
        for(int i = 0; i < 255; ++i){
            material.color -= new Color32(0, 0, 0, 1);
            yield return null;
        } 
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(isInvisible) return;

        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerStay(Collider other)
    {
        if(isInvisible) return;
        
        base.OnTriggerStay(other);
    }
}
