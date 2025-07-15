using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class RunSelectTargetSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<AimTargetBufferComponent, SearchTargetEvent, TransformComponent>> _filter = default;
        readonly EcsPoolInject<AimComponent> _aimPool = default;
        readonly EcsPoolInject<AimTargetBufferComponent> _targetsPool = default;
        readonly EcsPoolInject<SearchTargetEvent> _eventPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var targetsBuffer = ref _targetsPool.Value.Get(entity);
                var targets = targetsBuffer.TargetsBuffer;

                if (targets == null || targets.Count == 0)
                {
                    // Нет видимых целей — удаляем AimComponent
                    if (_aimPool.Value.Has(entity))
                        _aimPool.Value.Del(entity);

                    _eventPool.Value.Del(entity);
                    continue;
                }

                ref var transformComp = ref _transformPool.Value.Get(entity);
                // Выбор ближайшей цели (можно сделать по другому критерию)
                Transform closest = null;
                float minDistance = float.MaxValue;

                Vector3 position = transformComp.Transform.position;

                foreach (var t in targets)
                {
                    float dist = Vector3.Distance(position, t.position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        closest = t;
                    }
                }

                if (closest != null)
                {
                    if (_aimPool.Value.Has(entity))
                        _aimPool.Value.Get(entity).Target = closest;
                    else
                        _aimPool.Value.Add(entity).Target = closest;
                }

                _eventPool.Value.Del(entity);
            }
        }
    }
}