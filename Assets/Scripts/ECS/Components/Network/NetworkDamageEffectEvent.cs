using Leopotam.EcsLite;
using MemoryPack;
using Statement;

namespace Client 
{
    [MemoryPackable]
    public partial struct NetworkDamageEffectEvent : IRequestable
    {
        public float Value;
        public string EntityKeyTarget;
        public string EntityKeySource;
        public DamageType DamageType;

        public void Request(EcsWorld world)
        {
            var state = BattleState.Instance;

            if (state.TryGetEntity(EntityKeyTarget, out int entity))
            {
                if (!world.GetPool<TakeDamageEvent>().Has(entity))
                {
                    world.GetPool<TakeDamageEvent>().Add(entity).DamageData = new System.Collections.Generic.List<TakeDamageData>();
                }

                ref var takeDamageComp = ref world.GetPool<TakeDamageEvent>().Get(entity);
                takeDamageComp.DamageData.Add(new TakeDamageData()
                {
                    Value = Value,
                    Type = DamageType,
                    SourceEntityKey = EntityKeySource,
                });
            }
        }
    }

    public enum DamageType { physic, solar, lightning, stasis, twilight}
}