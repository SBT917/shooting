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

    //�}�E�X�|�C���^�[�̕���������
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

    //�ړ��L�[���������Ƃ��̏���
    void OnStartMove(InputAction.CallbackContext context)
    {
        Vector3 value = context.ReadValue<Vector2>();
        move.Direction = value;
    }

    //�ړ��L�[�𗣂����Ƃ��̏���
    void OnEndMove(InputAction.CallbackContext context)
    {
        move.Direction = Vector3.zero;
    }

    //�������L�[���������Ƃ��̏���
    void OnStartInvisible(InputAction.CallbackContext context)
    {
        foreach(var l in launchers) { l.StopLaunch(); }
        invisible.StartInvisible();
    }

    //�������L�[�𗣂����Ƃ��̏���
    void OnEndInvisible(InputAction.CallbackContext context)
    {
        invisible.EndInvisible();
    }


    //���˃L�[���������Ƃ��̏���
    void OnStartLaunch(InputAction.CallbackContext context)
    {
        if (invisible.IsInvisible) return;
        foreach (var l in launchers) { l.StopLaunch(); }

        int n = int.Parse(context.action.name[context.action.name.Length - 1].ToString());
        launchers[n - 1].StartLaunch();
    }

    //���˃L�[��������Ƃ��̏���
    void OnStopLaunch(InputAction.CallbackContext context)
    {
        if (invisible.IsInvisible) return;

        int n = int.Parse(context.action.name[context.action.name.Length - 1].ToString());
        launchers[n - 1].StopLaunch();
    }

    //���`���[�W�L�[���������Ƃ��̏���
    void OnRecharge(InputAction.CallbackContext context)
    {
        if (invisible.IsInvisible) return;
        foreach (var l in launchers) { 
            l.StopLaunch();
            l.StartRecharge();
        }
    }

    
}
