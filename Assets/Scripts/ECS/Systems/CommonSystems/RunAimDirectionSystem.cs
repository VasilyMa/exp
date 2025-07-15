using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;

namespace Client 
{
    sealed class RunAimDirectionSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<AimDirectionComponent, TopTransformComponent>> _filter = default;
        readonly EcsPoolInject<AimDirectionComponent> _aimDirPool = default;
        readonly EcsPoolInject<AimTargetBufferComponent> _targetsBufferPool = default;
        readonly EcsPoolInject<SearchTargetEvent> _searchTargetEventPool = default;
        readonly EcsPoolInject<TopTransformComponent> _topTranformPool = default; 
        readonly int enemyLayerMask = LayerMask.GetMask("Enemy");

        const float multiplier = 1.0f;
        const float ViewDistance = 10f;
        const float ViewAngle = 30f;
        const int RayCount = 6; 

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var aimDir = ref _aimDirPool.Value.Get(entity);
                ref var transformComp = ref _topTranformPool.Value.Get(entity);

                if (transformComp.Transform == null || aimDir.Direction == Vector3.zero)
                    continue;

                Quaternion lookRotation = Quaternion.LookRotation(aimDir.Direction);
                transformComp.Transform.rotation = lookRotation;// Quaternion.Slerp(transformComp.Transform.rotation, lookRotation, 0.2f * Time.deltaTime * multiplier); // плавный поворот
                                                                                                                           // Подготовка
                List<Transform> visibleTargets = new List<Transform>();
                Vector3 origin = transformComp.Transform.position + Vector3.up * 0.5f; // если нужно немного поднять точку
                Vector3 forward = aimDir.Direction.normalized;
                float halfAngle = ViewAngle / 2f;

                // Выпуск лучей по дуге
                for (int i = 0; i < RayCount; i++)
                {
                    float lerp = i / (float)(RayCount - 1); // от 0 до 1
                    float angle = Mathf.Lerp(-halfAngle, halfAngle, lerp);

                    Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * forward;

                    if (Physics.Raycast(origin, dir, out RaycastHit hit, ViewDistance, enemyLayerMask))
                    {
                        if (!visibleTargets.Contains(hit.transform))
                            visibleTargets.Add(hit.transform);
                    }
#if UNITY_EDITOR
                    Debug.DrawRay(origin, dir * ViewDistance, Color.red, 0.05f);
#endif
                }

                // Сохраняем в буфер и запускаем ивент
                if (_targetsBufferPool.Value.Has(entity))
                {
                    _targetsBufferPool.Value.Get(entity).TargetsBuffer = visibleTargets;
                }
                else
                {
                    _targetsBufferPool.Value.Add(entity).TargetsBuffer = visibleTargets;
                }

                _searchTargetEventPool.Value.Add(entity);
            }
        }
    }
}