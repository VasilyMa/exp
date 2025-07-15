using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Data/NewCharacter")]
public class CharacterBase : ScriptableObject, ISerializationCallbackReceiver
{
    public string KEY_ID;
    [SerializeReference] public PlayerCharacter Character;

    public PlayerCharacter GetCharacter()
    {
        return Character.Clone();
    }

    public PlayerCharacter LoadCharacter(CharacterData data)
    {
        var character = Character.Clone();
        character.Load(data);
        return character; 
    }


    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        if (Character != null)
        {
            KEY_ID = name;
            Character.KEY_ID = name;
        }
    }
}
