using Leopotam.EcsLite;

namespace Client
{
    public interface IDisposable
    {
        void DisposeComponent(EcsWorld world, int entity);
    }
}
