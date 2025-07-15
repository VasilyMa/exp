[System.Serializable]
public class PlayerData : IDatable
{
    public PlayerData()
    {
	
    }

    public string DATA_ID => "PlayerData_ID"; // name data

    public void ProcessUpdateData()
    {
        // TODO: Add data update logic
    }

    public void Dispose()
    {
        // TODO: Remove data to default values, invokes where Clear Data
    }

    public void ValidData()
    { 
    }
}