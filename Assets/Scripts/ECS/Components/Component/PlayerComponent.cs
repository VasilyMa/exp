using Leopotam.EcsLite;

namespace Client 
{
    struct PlayerComponent : IComponent
    {
        public void AddComponent(EcsWorld world, int entity)
        {
            world.GetPool<PlayerComponent>().Add(entity);
        }
    }
}