using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//アタッチしたオブジェクトにプレイヤーが近づくとカメラがオブジェクトにフォーカスするようになる。
public class FocusCamera: MonoBehaviour
{
    [SerializeField]private CinemachineVirtualCamera cinemachineCamera;
    [SerializeField]private CinemachineTargetGroup targetGroup;
    [SerializeField]private float inRange; //フォーカスを開始するプレイヤーの距離
    [SerializeField]float outRange; //フォーカスをやめるプレイヤーの距離
    private GameObject player;

    void Start() 
    {
        cinemachineCamera.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        targetGroup.m_Targets[1].target = player.transform;
    }

    void Update() 
    {
        if(Vector3.Distance(transform.position, player.transform.position) < inRange){
            cinemachineCamera.gameObject.SetActive(true);
        }
        else if(Vector3.Distance(transform.position, player.transform.position) > outRange){
            cinemachineCamera.gameObject.SetActive(false);
        }
    }
}
