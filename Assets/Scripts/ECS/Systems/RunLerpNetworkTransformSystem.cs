using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class RunLerpNetworkTransformSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<LerpTransformComponent, TransformComponent, TopTransformComponent>, Exc<OwnComponent>> _filter = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<LerpTransformComponent> _targetPool = default;
        readonly EcsPoolInject<TopTransformComponent> _topTransformPool = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var transformComp = ref _transformPool.Value.Get(entity);
                ref var topTransformComp = ref _topTransformPool.Value.Get(entity);
                ref var targetComp = ref _targetPool.Value.Get(entity);

                transformComp.Transform.position = Vector3.Lerp(
                    transformComp.Transform.position,
                    targetComp.TargetPosition,
                    Time.deltaTime * 10f // множитель скорости
                );

                transformComp.Transform.rotation = Quaternion.Slerp(
                    transformComp.Transform.rotation,
                    targetComp.TargetRotation,
                    Time.deltaTime * 10f
                );

                topTransformComp.Transform.rotation = Quaternion.Slerp(
                    topTransformComp.Transform.rotation,
                    targetComp.TargetTopRotation,
                    Time.deltaTime * 10f
                );
            }
        }
    }
}