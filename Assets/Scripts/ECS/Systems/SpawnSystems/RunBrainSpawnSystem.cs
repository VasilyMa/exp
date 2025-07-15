using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class RunBrainSpawnSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<SpawnComponent>> _filter = default;
        readonly EcsPoolInject<SpawnComponent> _spawnPool = default;
        readonly EcsPoolInject<SendSpawnEvent> _sendSpawnPool = default;
        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var spawnComp = ref _spawnPool.Value.Get(entity);

                if (spawnComp.Delay > 0)
                {
                    spawnComp.Delay -= Time.deltaTime;
                    continue;
                } 

                spawnComp.SpawnTimeRemaining -= Time.deltaTime;

                if (spawnComp.SpawnTimeRemaining <= 0)
                {
                    spawnComp.SpawnTimeRemaining = spawnComp.SpawnTick;

                    int random = Random.Range(0, spawnComp.SpawnPoints.Count);

                    Vector3 spawnPos = spawnComp.SpawnPoints[random].position;

                    ref var sendSpawnComp = ref _sendSpawnPool.Value.Add(_world.Value.NewEntity());
                    sendSpawnComp.SpawnPos = spawnPos;
                }
            }
        }
    }
}