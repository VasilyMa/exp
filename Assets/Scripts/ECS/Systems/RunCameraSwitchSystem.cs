using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunCameraSwitchSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<CameraComponent, CameraSwitchEvent>> _filter = default;
        readonly EcsPoolInject<CameraComponent> _cameraPool = default;
        readonly EcsPoolInject<CameraSwitchEvent> _switchPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var cameraSwitchComp = ref _switchPool.Value.Get(entity);
                ref var cameraComp = ref _cameraPool.Value.Get(entity);

                cameraComp.Camera.Follow = cameraSwitchComp.Target;
                cameraComp.Camera.LookAt = cameraSwitchComp.Target;

                _switchPool.Value.Del(entity);
            }
        }
    }
}