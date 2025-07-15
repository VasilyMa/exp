public class SentinelCharacter : PlayerCharacter
{
    public SentinelCharacter(SentinelCharacter data) : base(data)
    {

    }

    public override PlayerCharacter Clone()
    {
        return new SentinelCharacter(this);
    }
    public override CharacterData GetData()
    {
        return new CharacterData(KEY_ID, Index);
    }
}

