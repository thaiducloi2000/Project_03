using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using static UnityEditor.VersionControl.Asset;
using static UnityEngine.EventSystems.EventTrigger;

public struct AgentData
{
    public Transform spawnPoint;
    public bool isPartroll;
    public AgentStats stats;
}

[Serializable]
public enum AgentStats
{
    DIE = -1,
    RESPAWN,
    IDLE,
    MOVE,
    ATTACK,
    COUNTDOWN_ATTACK,
}

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour, ISpawnable
{
    public Transform spawnPoint { get; private set; }

    public bool isPartroll { get; private set; }

    [SerializeField] protected Transform target;
    public bool HasTarget => target != null;

    [SerializeField] protected float rangeAttack = 1f;

    [SerializeField] protected float attackSpeed;
    public float AttackSpeed => attackSpeed <= 0 ? 1 / 0.1f : 1 / attackSpeed;

    protected NavMeshAgent agent;
    protected EnemyAnimatorController animatorController;
    public EnemyAnimatorController AnimatorController => animatorController;
    public bool hasAnimator;

    //Enemy State

    public StateMachine state { get; private set; }
    // End of Enemy State

    private IObjectPool<ISpawnable> objectPool;

    public bool IsInRangeAttack => target != null && Vector3.Distance(this.transform.position, target.position) <= rangeAttack;

    private int hp = 1;

    public Transform SpawnObject => this.transform;
    public int HP => hp;

    public bool IsVisible { get; private set; }

    public bool IsInCountDown { get; private set; }

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        animatorController = GetComponentInChildren<EnemyAnimatorController>();
        hasAnimator = animatorController != null;

        LoadState();
    }
    private void LoadState()
    {
        state = new StateMachine(new Dictionary<Type, IState>()
        {
            { typeof(AI_CountDownAttackState), new AI_CountDownAttackState(this)},
            { typeof(AI_MoveState), new AI_MoveState(this)},
            { typeof(AI_IdleState), new AI_IdleState(this)},
            { typeof(AI_DeathState), new AI_DeathState(this)},
            { typeof(AI_RespawnState), new AI_RespawnState(this)},
            { typeof(AI_AttackState), new AI_AttackState(this)},
        });
    }

    // Start is called before the first frame update
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            GetDamaged(0f);
        }

        state.Update();
    }

    public virtual void Spawn(AgentData data)
    {
        spawnPoint = data.spawnPoint;
        isPartroll = data.isPartroll;
        state.TransitionTo(typeof(AI_RespawnState));
        hp = 1;
    }

    public void MoveToTarGet(bool isStandalone = false)
    {
        if (!agent.isOnNavMesh) return;
        agent.SetDestination(isStandalone ? transform.position : target.position);
    }

    public void SpawnDone()
    {
        IsVisible = false;
        state.TransitionTo(typeof(AI_IdleState));
    }

    public void Death()
    {
        objectPool.Release(this);
        state.TransitionTo(typeof(AI_RespawnState));
    }

    public void GetDamaged(float damage)
    {
        //test
        if (IsVisible) return;
        hp = 0;
        state.TransitionTo(typeof(AI_DeathState));
    }

    public ISpawnable OnCreate()
    {
        ISpawnable enemy = Instantiate(this);
        return enemy;
    }

    public void OnRelease(ISpawnable obj)
    {
        obj.SpawnObject.gameObject.SetActive(false);
    }

    public void OnGet(ISpawnable obj)
    {
        IsVisible = true;
        hp = 1;
        obj.SpawnObject.gameObject.SetActive(true);
    }

    public void OnDestroyFromPool(ISpawnable obj)
    {
        state.ClearListener();
        Destroy(obj.SpawnObject.gameObject);
    }

    public void SetUpPool(IObjectPool<ISpawnable> pool)
    {
        if (objectPool != null && objectPool == pool) return;
        objectPool = pool;
    }

    public void OnDoneCountDownAttack()
    {
        state.TransitionTo(typeof(AI_CountDownAttackState));
    }

    public void StartCountDown()
    {
        if (IsInCountDown) return; // Avoid recallback when calllback not done by before
        IsInCountDown = true;
        Invoke(nameof(OnCountDownDone), 1 / AttackSpeed);
    }

    private void OnCountDownDone()
    {
        IsInCountDown = false;
    }
}
