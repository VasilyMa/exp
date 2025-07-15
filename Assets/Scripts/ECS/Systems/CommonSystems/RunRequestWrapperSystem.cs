using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunRequestWrapperSystem<T> : IEcsRunSystem where T : struct, IRequestable 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<T, RequestEvent>> _filter = default;
        readonly EcsPoolInject<T> _pool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var requestComp = ref _pool.Value.Get(entity);
                requestComp.Request(_world.Value);

                _world.Value.DelEntity(entity);
            }
        }
    }
}