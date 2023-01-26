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
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    public void OnStartButton(Button button)
    {
        button.enabled = false; 
        StartCoroutine(Load());
    }

    public void OnSettingButton()
    {
        settingMenu.SetActive(true);
        buttons.SetActive(false);
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
        yield return new WaitForSeconds(audioSource.clip.length); 
        TransitionKit.instance.transitionWithDelegate(transition);
    }
}
