public class AI_MoveState : Enemy_ActionableState
{
    public AI_MoveState(Enemy enemy) : base(enemy)
    {
        _enemy = enemy;
        stateID = 2;
    }

    public override void Enter()
    {
        _enemy.AnimatorController.DoAnimation(stateID, true);
    }

    public override void Exit()
    {
        _enemy.AnimatorController.DoAnimation(stateID);
    }

    public override void Update()
    {
        _enemy.MoveToTarGet();
        if (_enemy.HP <=0)
        {
            _enemy.state.TransitionTo(typeof(AI_DeathState));
        }

        if (_enemy.IsInRangeAttack && !_enemy.IsInCountDown)
        {
            _enemy.state.TransitionTo(typeof(AI_AttackState));
        }

        if(_enemy.IsInRangeAttack && _enemy.IsInCountDown)
        {
            _enemy.state.TransitionTo(typeof(AI_IdleState));
        }
    }
}
