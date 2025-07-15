using Leopotam.EcsLite;

namespace Client 
{
    struct NonPlayerControlComponent : IComponent
    {
        public void AddComponent(EcsWorld world, int entity)
        {
            world.GetPool<NonPlayerControlComponent>().Add(entity);
        }
    }
}