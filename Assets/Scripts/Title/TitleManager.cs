using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;   
    }

    void Update()
    {
        
    }

    public void StartButton()
    {
        SceneManager.LoadScene("MainScene");
    }
}
