using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.WSA;

public class PlayerInputController : MonoBehaviour
{
    PlayerInput input;
    IMoveable move;
    IInvisible invisible;
    [SerializeField] List<ShotLauncher> launchers;

    private void Awake()
    {
        TryGetComponent(out input);
        TryGetComponent(out move);
        TryGetComponent(out invisible);
    }

    private void OnEnable()
    {
        input.actions["Move"].performed += OnStartMove;
        input.actions["Move"].canceled += OnEndMove;

        input.actions["Invisible"].started += OnStartInvisible;
        input.actions["Invisible"].canceled += OnEndInvisible;

        input.actions["Launch1"].performed += OnStartLaunch;
        input.actions["Launch1"].canceled += OnStopLaunch;
        input.actions["Launch2"].performed += OnStartLaunch;
        input.actions["Launch2"].canceled += OnStopLaunch;

        input.actions["Recharge"].performed += OnRecharge;
    }

    private void OnDisable()
    {
        input.actions["Move"].performed -= OnStartMove;
        input.actions["Move"].canceled -= OnEndMove;

        input.actions["Invisible"].started -= OnStartInvisible;
        input.actions["Invisible"].canceled -= OnEndInvisible;

        input.actions["Launch1"].performed -= OnStartLaunch;
        input.actions["Launch1"].canceled -= OnStopLaunch;
        input.actions["Launch2"].performed -= OnStartLaunch;
        input.actions["Launch2"].canceled -= OnStopLaunch;

        input.actions["Recharge"].performed -= OnRecharge;
    }

    private void Update()
    {
        LookMousePoint();
    }

    //マウスポインターの方向を見る
    private void LookMousePoint()
    {
        if (launchers.Find(l => l.IsShooting) == null) return;
        if (input.currentControlScheme != "Keyboard&Mouse") return;

        Plane plane = new Plane();
        float distance = 0;

        Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
        plane.SetNormalAndPosition(Vector3.up, transform.localPosition);
        if (plane.Raycast(ray, out distance))
        {
            Vector3 lookPoint = ray.GetPoint(distance);
            transform.LookAt(lookPoint);
        }
    }

    //移動キーを押したときの処理
    void OnStartMove(InputAction.CallbackContext context)
    {
        Vector3 value = context.ReadValue<Vector2>();
        move.Direction = value;
    }

    //移動キーを離したときの処理
    void OnEndMove(InputAction.CallbackContext context)
    {
        move.Direction = Vector3.zero;
    }

    //透明化キーを押したときの処理
    void OnStartInvisible(InputAction.CallbackContext context)
    {
        foreach(var l in launchers) { l.StopLaunch(); }
        invisible.StartInvisible();
    }

    //透明化キーを離したときの処理
    void OnEndInvisible(InputAction.CallbackContext context)
    {
        invisible.EndInvisible();
    }


    //発射キーを押したときの処理
    void OnStartLaunch(InputAction.CallbackContext context)
    {
        if (invisible.IsInvisible) return;
        foreach (var l in launchers) { l.StopLaunch(); }

        int n = int.Parse(context.action.name[context.action.name.Length - 1].ToString());
        launchers[n - 1].StartLaunch();
    }

    //発射キーを放したときの処理
    void OnStopLaunch(InputAction.CallbackContext context)
    {
        if (invisible.IsInvisible) return;

        int n = int.Parse(context.action.name[context.action.name.Length - 1].ToString());
        launchers[n - 1].StopLaunch();
    }

    //リチャージキーを押したときの処理
    void OnRecharge(InputAction.CallbackContext context)
    {
        if (invisible.IsInvisible) return;
        foreach (var l in launchers) { 
            l.StopLaunch();
            l.StartRecharge();
        }
    }

    
}
