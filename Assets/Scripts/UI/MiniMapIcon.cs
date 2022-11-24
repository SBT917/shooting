using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapIcon : MonoBehaviour
{
    [SerializeField] private Camera miniMapCamera;
    [SerializeField] private Transform parentObject;
    [SerializeField] private float rangeRadiusOffset;
    private float miniMapRangeRadius;
    private float defaultPosY;

    void Start()
    {
        miniMapCamera = GameObject.FindWithTag("MiniMapCamera").GetComponent<Camera>();
        parentObject = transform.root.gameObject.transform;
        rangeRadiusOffset = 3.0f;
        miniMapRangeRadius = miniMapCamera.orthographicSize;
        defaultPosY = transform.position.y;
    }

    void LateUpdate()
    {
        DispIcon();
        transform.rotation = Quaternion.Euler(90, transform.position.y, 0);
    }

    private void DispIcon()
    {
        Vector3 iconPos = new Vector3(parentObject.position.x, defaultPosY, parentObject.position.z);   

        if(CheckInsideMap()){
            transform.position = iconPos;
        }
        
        Vector3 centerPos = new Vector3(miniMapCamera.transform.position.x, defaultPosY, miniMapCamera.transform.position.z);
        Vector3 offset = transform.position - centerPos;
        transform.position = centerPos + Vector3.ClampMagnitude(offset, miniMapRangeRadius - rangeRadiusOffset);
    }

    private bool CheckInsideMap()
    {
        Vector3 cameraPos = miniMapCamera.transform.position;
        Vector3 iconPos = transform.position;

        cameraPos.y = 0;
        iconPos.y = 0;

        return Vector3.Distance(cameraPos, iconPos) <= miniMapRangeRadius - rangeRadiusOffset;
    }
}
