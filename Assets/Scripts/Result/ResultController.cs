using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Prime31.TransitionKit;

public class ResultController : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI waveText;
    [SerializeField]private TextMeshProUGUI scoreText;
    [SerializeField]private GameObject returnTitleText;

    [SerializeField]private Color transitionColor;

    void Awake()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        waveText.text = "WAVE:" + GameManager.finalWave.ToString();
        scoreText.text = "SCORE:" + GameManager.finalScore.ToString();
        InvokeRepeating("FlashText", 0.5f, 0.5f);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1)){
            SquaresTransition transition = new SquaresTransition()
            {
                squareColor = transitionColor,
                nextScene = 0,
            };
            TransitionKit.instance.transitionWithDelegate(transition);
        }
    }

    private void FlashText()
    {
        returnTitleText.SetActive(!returnTitleText.activeSelf);
    }
}
