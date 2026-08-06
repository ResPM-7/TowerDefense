using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    private IAttackStrategy strategy;

    private IAttackStrategy sword;
    private IAttackStrategy bow;

    private void Awake()
    {
        sword = new SwordAttack();
        bow = new BowAttack();

        strategy = sword;
    }

    public void Attack()
    {
        strategy?.Attack();
    }

    public void EquipSword()
    {
        strategy = sword;
    }

    public void EquipBow()
    {
        strategy = bow;
    }
}
