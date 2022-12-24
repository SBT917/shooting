using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController: MonoBehaviour
{
    [SerializeField]RectTransform cursorPos;
    CinemachineVirtualCamera virtualCamera;
    Vector3 cameraPos;
    Vector3 cameraRotate;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void LateUpdate()
    {
        cameraRotate = new Vector3(70, cursorPos.rect.position.x, 0);
        cameraRotate.y = Mathf.Clamp(cameraRotate.y, -30, 30);
        virtualCamera.transform.position = cameraPos;
        virtualCamera.transform.eulerAngles = cameraRotate;
    }
}
