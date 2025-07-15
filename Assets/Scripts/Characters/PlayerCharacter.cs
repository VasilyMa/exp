public abstract class PlayerCharacter : SourceCharacter
{
    [UnityEngine.HideInInspector] public int Index;

    public PlayerCharacter(PlayerCharacter data) : base(data)
    {

    }

    public abstract PlayerCharacter Clone();

    public abstract CharacterData GetData();

    public PlayerCharacter Load(CharacterData data)
    {


        return this;
    }
}
