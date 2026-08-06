using UnityEngine;

public class Monster : MonoBehaviour
{
    private StateMachine stateMachine;

    public IState idleState;
    public IState attackState;

    private void Awake()
    {
        stateMachine = new StateMachine();

        idleState = new IdleState(this);
        attackState = new AttackState(this);
    }

    private void Start()
    {
        stateMachine.ChangeState(idleState);
    }

    private void Update()
    {
        stateMachine.Repeat();
    }

    public void ChangeState(IState state)
    {
        stateMachine.ChangeState(state);
    }
}
