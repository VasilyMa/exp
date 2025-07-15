using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MemoryPack;
using System;

namespace Client 
{
    sealed class RunSendSpawnSystem : IEcsRunSystem 
    { 
        readonly EcsFilterInject<Inc<SendSpawnEvent>> _filter = default;
        readonly EcsPoolInject<SendSpawnEvent> _sendSpawnPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var sendSpawnComp = ref _sendSpawnPool.Value.Get(entity);

                var entityConfig = ConfigModule.GetConfig<EntityConfig>();

                if (entityConfig.TryGetEntity("default_enemy_1", out EntityBase entityBase))
                {
                    string entityKey = Guid.NewGuid().ToString(); 

                    NetworkUnitEntitySpawnEvent networkUnitEntitySpawnEvent = new NetworkUnitEntitySpawnEvent()
                    {
                        SpawnPos = sendSpawnComp.SpawnPos,
                        SpawnKeyID = entityBase.KEY_ID,
                        EntityKey = entityKey,
                    };

                    PhotonRunHandler.Instance.SendUnitEntitySpawnRPC(MemoryPackSerializer.Serialize(networkUnitEntitySpawnEvent));
                } 
            }
        }
    }
}