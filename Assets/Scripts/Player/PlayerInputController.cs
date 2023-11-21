using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInputController : MonoBehaviour
{
    PlayerInput input;
    IMoveable move;
    IInvisible invisible;


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
    }

    private void OnDisable()
    {
        input.actions["Move"].performed -= OnStartMove;
        input.actions["Move"].canceled -= OnEndMove;

        input.actions["Invisible"].started -= OnStartInvisible;
        input.actions["Invisible"].canceled -= OnEndInvisible;
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
        invisible.StartInvisible();
    }

    //�������L�[�𗣂����Ƃ��̏���
    void OnEndInvisible(InputAction.CallbackContext context)
    {
        invisible.EndInvisible();
    }
}
