using Leopotam.EcsLite;

namespace Client
{
    public interface IComponent
    {
        void AddComponent(EcsWorld world, int entity);
    }
}