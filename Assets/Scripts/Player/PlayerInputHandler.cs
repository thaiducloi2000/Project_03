using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Vector2 direction;
    public Vector2 Direction => direction;

    public void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
        Debug.Log(Direction);
    }
}
