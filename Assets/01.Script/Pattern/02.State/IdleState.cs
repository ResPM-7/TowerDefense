using UnityEngine;

public class IdleState : IState
{
    private Monster monster;

    public IdleState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }
}
