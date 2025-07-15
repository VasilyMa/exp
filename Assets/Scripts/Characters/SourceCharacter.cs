using UnityEngine;

public abstract class SourceCharacter
{
    public string KEY_ID;
    public Sprite CharacterIcon;

    public SourceCharacter(SourceCharacter data)
    {
        KEY_ID = data.KEY_ID;
        CharacterIcon = data.CharacterIcon;
    }
}
