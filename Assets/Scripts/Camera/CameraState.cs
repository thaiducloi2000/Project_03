using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraState : IState
{
    protected CinemachineController cameraController;
    protected CinemachineVirtualCamera _camera;
    protected const int activePriority = 10;
    protected const int deActivePriority = 1;

    public CameraState(CinemachineController cameraController, CinemachineVirtualCamera camera)
    {
        this.cameraController = cameraController;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void Update();
}

public class Camera_TopDownState : CameraState
{
    public Camera_TopDownState(CinemachineController cameraController, CinemachineVirtualCamera camera) : base(cameraController, camera)
    {
        base.cameraController = cameraController;
        _camera = camera;
    }

    public override void Enter()
    {
        _camera.Priority = activePriority;
        _camera.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        _camera.Priority = deActivePriority;
        _camera.gameObject.SetActive(false);
    }

    public override void Update()
    {
        if (!cameraController.isTopdownCamera)
        {
            cameraController.cameraStateMachine.TransitionTo(typeof(Camera_NormalState));
        }
    }
}

public class Camera_NormalState : CameraState
{
    public Camera_NormalState(CinemachineController cameraController, CinemachineVirtualCamera camera) : base(cameraController, camera)
    {
        base.cameraController = cameraController;
        base._camera = camera;
    }

    public override void Enter()
    {
        _camera.Priority = activePriority;
        _camera.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        _camera.Priority = deActivePriority;
        _camera.gameObject.SetActive(false);
    }

    public override void Update()
    {
        if (cameraController.isTopdownCamera)
        {
            cameraController.cameraStateMachine.TransitionTo(typeof(Camera_TopDownState));
        }
        if (cameraController.isAiming)
        {
            cameraController.cameraStateMachine.TransitionTo(typeof(Camera_AimState));
        }
    }
}

public class Camera_AimState : CameraState
{
    public Camera_AimState(CinemachineController cameraController, CinemachineVirtualCamera camera) : base(cameraController, camera)
    {
        base.cameraController = cameraController;
        base._camera = camera;
    }

    public override void Enter()
    {
        _camera.Priority = activePriority;
        _camera.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        _camera.Priority = deActivePriority;
        _camera.gameObject.SetActive(false);
    }

    public override void Update()
    {
        if (cameraController.isTopdownCamera)
        {
            cameraController.cameraStateMachine.TransitionTo(typeof(Camera_TopDownState));
        }
        if (!cameraController.isAiming)
        {
            cameraController.cameraStateMachine.TransitionTo(typeof(Camera_NormalState));
        }
    }
}
