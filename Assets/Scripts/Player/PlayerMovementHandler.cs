using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInputHandler))]
public class PlayerMovementHandler : MonoBehaviour, ITarget
{
    [Tooltip("Move speed of the character in m/s")]
    [SerializeField] private float MoveSpeed = 2.0f;

    [Tooltip("How fast the character turns to face movement direction")]
    [SerializeField, Range(0.0f, 0.3f)] private float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    [SerializeField] private float SpeedChangeRate = 10.0f;
    //[SerializeField] private float scanRadius = 20f;
    [SerializeField] private LayerMask scanLayer;

    [SerializeField] private Test target;

    [SerializeField] private Projectile bullet;
    [SerializeField] private AbilityInfor infor;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private CinemachineController mainCamera;
    [SerializeField] private AbilityType type;

    private PlayerInputHandler _input;
    private float _speed;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;

    private GameObject _mainCamera;
    private CharacterController _controller;

    public Vector3 Position => TargetTransform.position;

    public Transform TargetTransform => this.transform;

    public TargetStat TargetStart => throw new System.NotImplementedException();

    public Vector3 CenterPosition => centerPoint.position;

    private ProjecttileAbility ability;

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Helper.MainCamera.gameObject;
        }
        mainCamera = Helper.MainCamera.GetComponent<CinemachineController>();
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInputHandler>();
        ability = new ProjecttileAbility(infor, new SpreadTrajector(infor.amountWave, 60f), bullet);

        _input.OnChangeCameraCallBack += mainCamera.ChangeCamera;
        _input.OnAiming += mainCamera.AimChange;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ability.UseAbility(this, target);
        }
        Move();
    }

    private void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_input.Direction == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = _input.Direction.magnitude;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        Vector3 inputDirection = new Vector3(_input.Direction.x, 0.0f, _input.Direction.y).normalized;

        if (_input.Direction != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }
}
