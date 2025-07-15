using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client 
{
    sealed class InitCameraSystem : IEcsInitSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<CameraComponent> _cameraPool = default;

        public void Init (IEcsSystems systems) 
        {
            var entity = _world.Value.NewEntity();
            
            ref var cameraComp = ref _cameraPool.Value.Add(entity);
            cameraComp.Camera = GameObject.FindFirstObjectByType<Cinemachine.CinemachineVirtualCamera>();

            BattleState.Instance.AddEntity("camera", entity);
        }
    }
}