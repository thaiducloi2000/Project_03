using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class EnemyAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private const string ANIMATOR_STATE_DIE = "DoDeadth";
    private const string ANIMATOR_STATE_MOVE = "isMove";
    private const string ANIMATOR_STATE_ATTACK = "Attack";

    private int Animator_DoDie = Animator.StringToHash(ANIMATOR_STATE_DIE);
    private int Animator_DoMove = Animator.StringToHash(ANIMATOR_STATE_MOVE);
    private int Animator_Attack = Animator.StringToHash(ANIMATOR_STATE_ATTACK);

    private Dictionary<AgentStats, UnityAction<bool>> animatorAction = new Dictionary<AgentStats, UnityAction<bool>>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        LoadAnimator();

    }

    private void LoadAnimator()
    {
        animatorAction.Add(AgentStats.DIE, DoDie);
        animatorAction.Add(AgentStats.MOVE, DoMove);
        animatorAction.Add(AgentStats.ATTACK, DoAttack);
    }

    public void DoAnimation(int stats, bool isActive = false)
    {
        animator.ResetTrigger(Animator_DoDie);
        if (animatorAction.ContainsKey((AgentStats) stats))
        {
            animatorAction[(AgentStats) stats]?.Invoke(isActive);
        }
    }

    private void DoMove(bool isActive)
    {
        animator.SetBool(Animator_DoMove, isActive);
    }

    private void DoDie(bool isActive = false)
    {
        animator.SetTrigger(Animator_DoDie);
    }

    private void DoAttack(bool isActive)
    {
        animator.SetBool(Animator_Attack, isActive);
    }

    public void ResetStat()
    {
        animatorAction[AgentStats.MOVE]?.Invoke(false);
        animatorAction[AgentStats.ATTACK]?.Invoke(false); 
    }
}
