using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using UnityEngine;

namespace Client
{
    sealed class RunInputMovementSystem : IEcsRunSystem
    { 
        readonly EcsFilterInject<Inc<InputMovementComponent, InputComponent>> _filter = default;
        readonly EcsFilterInject<Inc<PlayerComponent, OwnComponent>> _playerFilter = default;
        readonly EcsPoolInject<InputMovementComponent> _inputPool = default;
        readonly EcsPoolInject<DirectionComponent> _directionPool = default; 

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            { 
                foreach (var playerEntity in _playerFilter.Value)
                { 
                    ref var inputComp = ref _inputPool.Value.Get(entity);

                    float horizontal = inputComp.MovementJoystick.Horizontal;
                    float vertical = inputComp.MovementJoystick.Vertical;

                    if (horizontal != 0 || vertical != 0)
                    {
                        Vector3 direction = new Vector3(horizontal, 0f, vertical);

                        if (_directionPool.Value.Has(playerEntity))
                        {
                            ref var directionComp = ref _directionPool.Value.Get(playerEntity);
                            directionComp.Direction = direction;
                        }
                        else
                        {
                            ref var directionComp = ref _directionPool.Value.Add(playerEntity);
                            directionComp.Direction = direction;
                        }
                    }
                    else
                    {
                        if (_directionPool.Value.Has(playerEntity))
                        {
                            _directionPool.Value.Del(playerEntity);
                        }
                    } 
                }
            }
        }
    }
}
