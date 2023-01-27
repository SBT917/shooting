using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Prime31.TransitionKit;

public class TitleManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]private Color transitionColor;
    [SerializeField]private GameObject settingMenu;
    [SerializeField]private GameObject buttons;

    void Start()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        buttons.SetActive(!settingMenu.activeSelf);
    }

    public void OnStartButton(Button button)
    {
        button.enabled = false; 
        StartCoroutine(Load());
    }

    public void OnSettingButton()
    {
        settingMenu.SetActive(true);
    }
    
    public void OnExitButton()
    {
        Application.Quit();
    }

    public IEnumerator Load()
    {
        audioSource.Play();
        SquaresTransition transition = new SquaresTransition()
        {
            squareColor = transitionColor,
            nextScene = 1
        };
        yield return new WaitForSeconds(1.5f); 
        TransitionKit.instance.transitionWithDelegate(transition);
    }
}
