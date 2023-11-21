using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�V���O���g���̃W�F�l���b�N�N���X
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
