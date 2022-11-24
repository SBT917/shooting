using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    public GameObject targetEnemy;
    public List<GameObject> targetEnemys;

    void Update()
    {
        if(targetEnemy != null){
            if(!targetEnemy.activeSelf){
                targetEnemy = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy")){
            targetEnemy = other.gameObject;
            targetEnemys.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Enemy")){
            targetEnemys.Remove(other.gameObject);
        }
    }
}
