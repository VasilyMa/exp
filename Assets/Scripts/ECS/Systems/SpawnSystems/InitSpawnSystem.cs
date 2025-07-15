using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Linq;
using UnityEngine;

namespace Client 
{
    sealed class InitSpawnSystem : IEcsInitSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<SpawnComponent> _spawnPool = default;

        public void Init (IEcsSystems systems) 
        {
            var entity = _world.Value.NewEntity();

            ref var spawnComp = ref _spawnPool.Value.Add(entity);

            Transform[] allObjects = GameObject.FindObjectsOfType<Transform>();
            int spawnLayer = LayerMask.NameToLayer("SpawnPoint");

            spawnComp.SpawnPoints = new System.Collections.Generic.List<Transform>();

            for (int i = 0; i < allObjects.Length; i++)
            {
                if (allObjects[i].gameObject.layer == spawnLayer)
                {
                    spawnComp.SpawnPoints.Add(allObjects[i]);
                }
            } 

            spawnComp.SpawnTick = 2f;
            spawnComp.Delay = 10f;
        }
    }
}