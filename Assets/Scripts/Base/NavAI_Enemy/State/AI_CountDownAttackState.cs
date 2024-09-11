using System.Diagnostics;

public class AI_CountDownAttackState : Enemy_ActionableState
{
    public AI_CountDownAttackState(Enemy enemy) : base(enemy)
    {
        _enemy = enemy;
        stateID = 1; // are being in wait => stand idle
    }

    public override void Enter()
    {
        _enemy.AnimatorController.DoAnimation(stateID, true);
        _enemy.StartCountDown();
    }

    public override void Exit()
    {
        _enemy.AnimatorController.DoAnimation(stateID);
    }

    /// <summary>
    /// Countdown will not translate to Idle because it is already idle state
    /// </summary>
    public override void Update()
    {
        if (_enemy.HP <= 0)
        {
            _enemy.state.TransitionTo(typeof(AI_DeathState));
        }

        if (_enemy.IsInRangeAttack && !_enemy.IsInCountDown)
        {
            _enemy.state.TransitionTo(typeof(AI_AttackState));
        }

        if (!_enemy.IsInRangeAttack)
        {
            _enemy.state.TransitionTo(typeof(AI_MoveState));
        }
    }
}
