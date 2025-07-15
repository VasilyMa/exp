public class ChaplainCharacter : PlayerCharacter
{
    public ChaplainCharacter(ChaplainCharacter data) : base(data)
    {

    }
    public override PlayerCharacter Clone()
    {
        return new ChaplainCharacter(this);
    }
    public override CharacterData GetData()
    {
        return new CharacterData(KEY_ID, Index);
    }
}
