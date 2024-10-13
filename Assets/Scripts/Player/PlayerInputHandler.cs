using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Vector2 direction;
    private bool isAim;
    public event Action OnChangeCameraCallBack;
    public Vector2 Direction => direction;
    public event Action<bool> OnAiming;

    public void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    public void OnChangeCamera(InputValue value)
    {
        if (value.isPressed)
        {
            OnChangeCameraCallBack?.Invoke();
        }
    }

    public void OnAim(InputValue value)
    {
        OnAiming?.Invoke(value.isPressed);
    }
}
