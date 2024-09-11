using UnityEngine;

public class RangeEnemy : Enemy
{
    public GameObject abilityCast;

    //protected override void Attack()
    //{
    //    if (Vector3.Distance(transform.position, target.position) > rangeAttack)
    //    {
    //        animatorController.DoAnimation(stats, false);
    //        UpdateStats(AgentStats.MOVE);
    //        return;
    //    }
    //    // uni task ?

    //    //if(stats)
    //}

    public void CastAbility()
    {
        // Spawn Ability

        //Back Stats to Idle
        //animatorController.DoAnimation(stats, false);
        state.TransitionTo(typeof(AI_CountDownAttackState));
    }
}
