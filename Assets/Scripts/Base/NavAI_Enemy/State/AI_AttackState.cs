public class AI_AttackState : Enemy_ActionableState
{
    public AI_AttackState(Enemy enemy) : base(enemy)
    {
        _enemy = enemy;
        stateID = 3;
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
        if (_enemy.IsInCountDown)
        {
            _enemy.state.TransitionTo(typeof(AI_CountDownAttackState));
        }
    }
}
