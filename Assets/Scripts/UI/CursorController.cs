using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static bool isDontDestroy;

    void Awake()
    {
        Cursor.visible = false;

        if(!isDontDestroy){
            DontDestroyOnLoad(gameObject.transform.root.gameObject);
            isDontDestroy = true;
        }
        else{
            Destroy(gameObject.transform.root.gameObject);
        }
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
