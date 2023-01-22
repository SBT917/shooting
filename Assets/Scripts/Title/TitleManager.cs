using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Prime31.TransitionKit;

public class TitleManager : MonoBehaviour
{
    [SerializeField]private Color transitionColor;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        
    }

    public void StartButton()
    {
        var transition = new SquaresTransition()
        {
            squareColor = transitionColor,
            nextScene = 1
        };
        TransitionKit.instance.transitionWithDelegate(transition);
    }
}
