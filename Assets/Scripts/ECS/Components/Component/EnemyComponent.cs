using Leopotam.EcsLite;

namespace Client 
{
    struct EnemyComponent : IComponent
    {
        public void AddComponent(EcsWorld world, int entity)
        {
            world.GetPool<EnemyComponent>().Add(entity);
        }
    }
}