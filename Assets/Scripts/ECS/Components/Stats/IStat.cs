using Leopotam.EcsLite;

public interface IStat
{
    public float GetMaxValue { get; }
    public float GetCurrentValue {  get; }
    void Init(EcsWorld world, int entity);
    void SetCurrent(float value);
    void Sub(float value);
    void Add(float value);
}