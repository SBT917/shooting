using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardCanvas : MonoBehaviour
{
    private Canvas canvas;
    void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    void Update()
    {
        canvas.transform.rotation = Camera.main.transform.rotation;
    }
}
