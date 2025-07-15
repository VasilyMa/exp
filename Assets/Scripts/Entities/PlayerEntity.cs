using UnityEngine;

public class PlayerEntity : SourceEntity
{
    public static PlayerEntity Instance;

    public string PlayfabCustomID;
    public string PlayerNickName;

    public int CurrentCharacterSlotIndex;

    public PlayerCharacter CurrentPlayerCharacter;
    public CharacterBase CurrentBaseCharacter;

    public PlayerEntity()
    {
        Instance = this;
    }

    public override void Init()
    {

    }

    public void CreateNewCharacter()
    {
        CurrentPlayerCharacter = CurrentBaseCharacter.GetCharacter();
        CurrentPlayerCharacter.Index = CurrentCharacterSlotIndex;
        Debug.Log($"Create new character {CurrentPlayerCharacter.KEY_ID}");
    }
}
