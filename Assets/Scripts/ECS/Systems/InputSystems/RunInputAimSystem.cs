using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client 
{
    sealed class RunInputAimSystem : IEcsRunSystem
    { 
        readonly EcsFilterInject<Inc<InputAimComponent, InputComponent>> _filter = default;
        readonly EcsFilterInject<Inc<PlayerComponent, OwnComponent>> _playerFilter = default;
        readonly EcsPoolInject<InputAimComponent> _inputPool = default;
        readonly EcsPoolInject<AimDirectionComponent> _aimDirectionPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                foreach (var playerEntity in _playerFilter.Value)
                {
                    ref var inputComp = ref _inputPool.Value.Get(entity);

                    float aimHorizontal = inputComp.AimJoystick.Horizontal;
                    float aimVertical = inputComp.AimJoystick.Vertical;

                    if (aimHorizontal != 0 || aimVertical != 0)
                    {
                        Vector3 aimDirection = new Vector3(aimHorizontal, 0f, aimVertical);

                        if (_aimDirectionPool.Value.Has(playerEntity))
                        {
                            _aimDirectionPool.Value.Get(playerEntity).Direction = aimDirection;
                        }
                        else
                        {
                            _aimDirectionPool.Value.Add(playerEntity).Direction = aimDirection;
                        }
                    }
                    else
                    {
                        _aimDirectionPool.Value.Del(playerEntity);
                    } 
                } 
            }
        }
    }
}