using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultController : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI waveText;
    [SerializeField]private TextMeshProUGUI scoreText;
    [SerializeField]private GameObject returnTitleText;

    void Awake()
    {
        Cursor.visible = true;
        waveText.text = "WAVE:" + GameManager.finalWave.ToString();
        scoreText.text = "SCORE:" + GameManager.finalScore.ToString();
        InvokeRepeating("FlashText", 0.5f, 0.5f);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1)){
            SceneManager.LoadScene("TitleScene");
        }
    }

    private void FlashText()
    {
        returnTitleText.SetActive(!returnTitleText.activeSelf);
    }
}
