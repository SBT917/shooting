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
        invisible.StartInvisible();
    }

    //透明化キーを離したときの処理
    void OnEndInvisible(InputAction.CallbackContext context)
    {
        invisible.EndInvisible();
    }
}
