using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Prime31.TransitionKit;

public class LoadManager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI text;
    [SerializeField]private Slider slider;

    void Start()
    {
        StartCoroutine(TextAnimaiton());
        StartCoroutine(LoadSlider());
    }

    private IEnumerator LoadSlider()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("MainScene");
    
        while(!async.isDone){
            slider.value = async.progress;
            yield return null;
        }
    }

    private IEnumerator TextAnimaiton()
    {
        while(true){
            text.text = "NOW LOADING";
            for(int i = 0; i < 4; ++i){
                yield return new WaitForSeconds(0.5f);
                text.text += ".";
            }
        }
    }
}
