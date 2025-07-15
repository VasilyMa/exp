public class AgentCharacter : PlayerCharacter
{
    public AgentCharacter(AgentCharacter data) : base(data)
    {

    }

    public override PlayerCharacter Clone()
    {
        return new AgentCharacter(this);
    }

    public override CharacterData GetData()
    {
        return new CharacterData(KEY_ID, Index);
    }
}
