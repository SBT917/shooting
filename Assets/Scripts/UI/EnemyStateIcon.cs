using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStateIcon : MonoBehaviour
{
    private Enemy parentEnemy;
    private Image stateIcon;
    [SerializeField]private Sprite[] icons;

    private EnemyState preState ;
    private EnemyState currentState;

    private Coroutine co;

    void Start()
    {
        stateIcon = GetComponent<Image>();
        parentEnemy = transform.root.gameObject.GetComponent<Enemy>();
        stateIcon.color = new Color32(255, 255, 255, 0);
    }

    
    void Update()
    {
        currentState = parentEnemy.GetState();
        
        if(preState != currentState){
            if(co != null) StopCoroutine(co);
            co = StartCoroutine(ChangeStateIcon(currentState));
        }

        preState = currentState;
    }

    private IEnumerator ChangeStateIcon(EnemyState state)
    {
        switch(state){
            case EnemyState.Attack:
                stateIcon.sprite = icons[0];
                break;
            case EnemyState.Search:
                stateIcon.sprite = icons[1];
                break;
            default:
                yield break;
        }

        stateIcon.color = new Color32(255, 255, 255, 255);

        yield return new WaitForSeconds(5.0f);
        for(int i = 0; i < 255; ++i){
            stateIcon.color -= new Color32(0, 0, 0, 2);
            yield return null;
        } 
    }
}
