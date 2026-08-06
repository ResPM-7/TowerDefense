
public class CharacterBuilder
{
    private readonly Character character = new Character();

    public CharacterBuilder SetName(string name)
    {
        character.Name = name;
        return this;
    }

    public CharacterBuilder SetHealth(int health)
    {
        character.Health = health;
        return this;
    }

    public CharacterBuilder SetAttack(int attack)
    {
        character.Attack = attack;
        return this;
    }

    public CharacterBuilder SetMoveSpeed(float moveSpeed)
    {
        character.MoveSpeed = moveSpeed;
        return this;
    }

    public CharacterBuilder SetWeapon(string weaponName)
    {
        character.WeaponName = weaponName;
        return this;
    }

    public Character Build()
    {
        return character;
    }
}
