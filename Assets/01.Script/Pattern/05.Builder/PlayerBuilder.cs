using UnityEngine;

public class PlayerBuilder : MonoBehaviour
{
    Character character = new CharacterBuilder()
    .SetName("Knight")
    .SetHealth(100)
    .SetAttack(30)
    .SetMoveSpeed(5f)
    .SetWeapon("Sword")
    .Build();
    void Start()
    {
        Debug.Log(character.Name);
    }

}
