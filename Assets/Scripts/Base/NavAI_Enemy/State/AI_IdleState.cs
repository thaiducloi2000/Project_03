public class AI_IdleState : Enemy_ActionableState
{
    public AI_IdleState(Enemy enemy) : base(enemy)
    {
        _enemy = enemy;
        stateID = 1;
    }

    public override void Enter()
    {
        _enemy.AnimatorController.DoAnimation(stateID, true);
        _enemy.MoveToTarGet(true);
    }

    public override void Exit()
    {
        _enemy.AnimatorController.DoAnimation(stateID);
    }

    public override void Update()
    {
        if (_enemy.HP <= 0)
        {
            _enemy.state.TransitionTo(typeof(AI_DeathState));
        }

        if (_enemy.IsInRangeAttack)
        {
            _enemy.state.TransitionTo(!_enemy.IsInCountDown ? typeof(AI_AttackState) : typeof(AI_CountDownAttackState));
        }

        if (!_enemy.IsInRangeAttack)
        {
            _enemy.state.TransitionTo(typeof(AI_MoveState));
        }
    }
}
