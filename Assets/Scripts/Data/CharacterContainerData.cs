using UnityEngine;

[System.Serializable]
public class CharacterContainerData : IDatable
{
    public CharacterData[] Characters = new CharacterData[] {new CharacterData(string.Empty, 0), new CharacterData(string.Empty, 1), new CharacterData(string.Empty, 2) };

    public CharacterContainerData()
    {
	    
    }

    public string DATA_ID => "CharacterContainerData_ID"; // name data

    public void ProcessUpdateData()
    {
        for (int i = 0; i < Characters.Length; i++)
        {
            var data = Characters[i];

            if (data.CHARACTER_KEY_ID == PlayerEntity.Instance.CurrentPlayerCharacter.KEY_ID)
            {
                Characters[i] = PlayerEntity.Instance.CurrentPlayerCharacter.GetData();
                break;
            }
        }

        Characters[PlayerEntity.Instance.CurrentCharacterSlotIndex] = PlayerEntity.Instance.CurrentPlayerCharacter.GetData();
        
        // TODO: Add data update logic
    }

    public void Dispose()
    {
        // TODO: Remove data to default values, invokes where Clear Data
    }

    public void ValidData()
    {
        for (int i = 0; i < Characters.Length; i++)
        {
            if (Characters[i] == null) Characters[i] = new CharacterData(string.Empty, i);
        }
    }
}

[System.Serializable]
public class CharacterData
{
    public int Index;
    public string CHARACTER_KEY_ID;

    public CharacterData(string characterKeyId, int index) 
    {
        CHARACTER_KEY_ID = characterKeyId;
        Index = index;
        Debug.Log($"Save character {CHARACTER_KEY_ID}");
    }
}