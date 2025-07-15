using System;
using UnityEngine;

public class ObserverEntity : SourceEntity
{
    public static ObserverEntity Instance;
    public event Action<float, float> PlayerHealthValueChanged;
    public event Action<EffectViewData> PlayerEffectAdded;
    public event Action<EffectViewData> PlayerEffectRemoved;

    public ObserverEntity()
    {
        Instance = this;
    }


    public override void Init()
    {

    }

    public void UpdatePlayerHealthValue(float current, float max)
    {
        PlayerHealthValueChanged?.Invoke(current, max); 
    }

    public void AddPlayerEffectValue(EffectViewData effectData) 
    {
        PlayerEffectAdded?.Invoke(effectData);
    }
    public void RemovePlayerEffectValue(EffectViewData effectData) 
    {
        PlayerEffectAdded?.Invoke(effectData);
    }
}

public class EffectViewData
{
    public event Action<float, float> OnRemainingChange;
    public Sprite Icon;
    public bool IsPositive;
    public int Weight;

    public void UpdateRemaining(float current, float max)
    {
        OnRemainingChange?.Invoke(current, max);
    }
}