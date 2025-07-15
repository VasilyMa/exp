using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MemoryPack;

namespace Client 
{
    sealed class RunSendUpdateHealthSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<HealthUpdateEvent>> _filter = default;
        readonly EcsPoolInject<HealthUpdateEvent> _healthUpdatePool = default;
        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var healthUpdateComp = ref _healthUpdatePool.Value.Get(entity);

                NetworkHealthUpdateEvent networkHealthUpdateEvent = new NetworkHealthUpdateEvent()
                {
                    CurrentValue = healthUpdateComp.CurrentValue,
                    EntityKey = healthUpdateComp.EntityKey,
                };

                PhotonRunHandler.Instance.SendRequestHealthUpdateRPC(MemoryPackSerializer.Serialize(networkHealthUpdateEvent));
            }
        }
    }
}