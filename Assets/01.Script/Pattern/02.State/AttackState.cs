using UnityEngine;

public class AttackState : IState
{
    private Monster monster;

    public AttackState(Monster monster)
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
