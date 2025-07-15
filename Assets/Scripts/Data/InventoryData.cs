using System.Collections.Generic;

[System.Serializable]
public class InventoryData : IDatable
{
    public List<InventorySlotData> _inventory = new List<InventorySlotData>();

    public InventoryData()
    {

    }

    public string DATA_ID => "InventoryData_ID"; // name data

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

[System.Serializable]
public class InventorySlotData
{
    public int SlotIndex;
    public ItemData ItemData;

    public InventorySlotData(int index, ItemData data)
    {
        SlotIndex = index;
        ItemData = data;
    }
}

[System.Serializable]
public abstract class ItemData
{
    public string KEY_ID;

    public ItemData(ISavable savable) 
    {
        KEY_ID = savable.ID;
    }
}