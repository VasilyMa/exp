using Leopotam.EcsLite;
using UnityEngine;

namespace Client 
{
    [MemoryPack.MemoryPackable]
    public partial struct NetworkUnitEntitySpawnEvent : IRequestable
    {
        public Vector3 SpawnPos;
        public int PlayerOwner;
        public string SpawnKeyID;
        public string EntityKey;

        public void Request(EcsWorld world)
        {
            ref var reciveSpawnComp = ref world.GetPool<ReceiveSpawnEvent>().Add(world.NewEntity());

            reciveSpawnComp.SpawnPos = SpawnPos;
            reciveSpawnComp.PlayerOwner = PlayerOwner;
            reciveSpawnComp.SpawnKeyID = SpawnKeyID;
            reciveSpawnComp.EntityKey = EntityKey; 
        }
    }
 
    /// <summary>
    /// This entity is networked
    /// </summary>
    public struct NetworkEntityComponent
    {
        public int PlayerOwner; 
        public string EntityKey;
    } 
}