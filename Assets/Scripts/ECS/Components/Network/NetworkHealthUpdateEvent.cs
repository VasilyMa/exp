using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    struct NetworkHealthUpdateEvent : IRequestable
    {
        public string EntityKey;
        public float CurrentValue;

        public void Request(EcsWorld world)
        {
            var state = BattleState.Instance;

            if (state.TryGetEntity(EntityKey, out int entity))
            {
                if (world.GetPool<HealthUpdateEvent>().Has(entity))
                {
                    ref var healthUpdateComp = ref world.GetPool<HealthUpdateEvent>().Get(entity);
                    healthUpdateComp.CurrentValue = CurrentValue;
                }
                else
                {
                    ref var healthUpdateComp = ref world.GetPool<HealthUpdateEvent>().Add(entity);
                    healthUpdateComp.CurrentValue = CurrentValue;
                } 
            }
        }
    }

    public struct HealthUpdateEvent
    {
        public string EntityKey;
        public float CurrentValue;
    }
}