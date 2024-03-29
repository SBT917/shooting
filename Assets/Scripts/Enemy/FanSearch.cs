using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSearch : MonoBehaviour
{   
    private float angle;
    private Player player;
    private Enemy parentEnemy;
    private SphereCollider sphereCollider;
    private float defaultColliderRadius;

    void Start() 
    {
        angle = transform.root.gameObject.GetComponent<Enemy>().enemyData.searchAngle;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        parentEnemy = transform.root.gameObject.GetComponent<Enemy>();
        sphereCollider = GetComponent<SphereCollider>();
        defaultColliderRadius = sphereCollider.radius;
    }

    void Update()
    {
        if(parentEnemy.GetState() == EnemyState.Search){
            sphereCollider.radius = defaultColliderRadius * 2;
        }
        else{
            sphereCollider.radius = defaultColliderRadius;
        }
    }

    public void OnTriggerStay(Collider other)
    { 
        if(parentEnemy.GetState() == EnemyState.Attack) return;
        
        Vector3 offset = new Vector3(0.0f, 0.5f, 0.0f);
        if(other.CompareTag("Player") && player.GetState() == PlayerState.Normal)
        {
            Vector3 posDelta = (other.transform.position + offset) - (transform.position + offset); 
			float playerAngle = Vector3.Angle(transform.forward, posDelta);
            Debug.DrawRay(transform.position + offset, posDelta, Color.red);
			if(playerAngle < angle)
			{
                if(Physics.Raycast(transform.position + offset, posDelta, out RaycastHit hit))
                { 
                    if(hit.collider == other)
                    {
                        parentEnemy.SetState(EnemyState.Attack);
                    }
                }
			} 
        }      
    }   
}
