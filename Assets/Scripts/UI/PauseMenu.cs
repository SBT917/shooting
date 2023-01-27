using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Prime31.TransitionKit;

public class PauseMenu : MonoBehaviour
{
    public void OnReturnTitleButton()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
