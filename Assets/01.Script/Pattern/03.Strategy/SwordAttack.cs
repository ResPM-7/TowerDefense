using UnityEngine;


public interface IAttackStrategy
{
    void Attack();
}

public class SwordAttack : IAttackStrategy
{
    public void Attack()
    {
        Debug.Log("°Ë °ø°Ý");
    }
}
