using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunDisposeSystem<T> : IEcsRunSystem where T : struct, IDisposable
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<T, DisposeEvent>> _filter = default;
        readonly EcsPoolInject<T> _pool = default;
        readonly EcsPoolInject<DisposeEvent> _dispose = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var disposeComp = ref _pool.Value.Get(entity);

                disposeComp.DisposeComponent(_world.Value, entity);

                _pool.Value.Del(entity);
                _dispose.Value.Del(entity);
            }
        }
    }
}