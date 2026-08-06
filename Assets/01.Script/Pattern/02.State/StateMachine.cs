using UnityEngine;


public interface IState
{
    void Enter();
    void Update();
    void Exit();
}


public class StateMachine
{
    public IState currentState;

    public void ChangeState(IState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState.Enter();
    }

    public void Repeat()
    {
        currentState?.Update();
    }

}
