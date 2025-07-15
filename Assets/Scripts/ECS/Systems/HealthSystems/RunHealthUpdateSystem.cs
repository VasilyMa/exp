using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunHealthUpdateSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<HealthUpdateEvent, HealthComponent>, Exc<DieComponent>> _filter = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<HealthUpdateEvent> _updatePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var healthComp = ref _healthPool.Value.Get(entity);
                ref var updateComp = ref _updatePool.Value.Get(entity);

                healthComp.SetCurrent(updateComp.CurrentValue); 
            }
        }
    }
}