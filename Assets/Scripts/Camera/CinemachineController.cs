using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera topDownCamera;
    [SerializeField] private CinemachineVirtualCamera nomalCamera;
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    public StateMachine cameraStateMachine { get; private set; }
    public bool isTopdownCamera { get; private set; }

    public bool isAiming { get; private set; }

    private void Awake()
    {
        LoadState();
    }

    private void LoadState()
    {
        cameraStateMachine = new StateMachine(new Dictionary<Type, IState>()
        {
            { typeof(Camera_NormalState), new Camera_NormalState(this,nomalCamera)},
            { typeof(Camera_TopDownState), new Camera_TopDownState(this,topDownCamera)},
            { typeof(Camera_AimState), new Camera_AimState(this,aimCamera)},
        });
    }

    private void Start()
    {
        isTopdownCamera = true;
        cameraStateMachine.TransitionTo(typeof(Camera_TopDownState));
    }

    // Update is called once per frame
    void Update()
    {
        cameraStateMachine.Update();
    }

    public void AimChange(bool isAim)
    {
        isAiming = isAim;
    }

    public void ChangeCamera()
    {
        isTopdownCamera = !isTopdownCamera;
    }
}
