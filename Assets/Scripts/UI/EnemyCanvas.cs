using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCanvas : MonoBehaviour
{
    private Canvas canvas;
    private Enemy parentEnemy;
    void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        gameObject.SetActive(false);
    }

    void Update()
    {
        canvas.transform.rotation = Camera.main.transform.rotation;
    }
}
