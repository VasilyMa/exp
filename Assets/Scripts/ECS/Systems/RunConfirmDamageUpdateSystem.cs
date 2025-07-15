using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunConfirmDamageUpdateSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<ConfirmDamageUpdateEvent>> _fitler = default;
        readonly EcsPoolInject<ConfirmDamageUpdateEvent> _confirmDamagePool = default;
        readonly EcsPoolInject<NetworkEntityComponent> _networkPool = default;

        public void Run (IEcsSystems systems) 
        { 
            foreach (var entity in _fitler.Value)
            {
                var state = BattleState.Instance;

                ref var confirmDamageComp = ref _confirmDamagePool.Value.Get(entity);

                if (state.TryGetEntity("player", out int playerEntity))
                {
                    if (_networkPool.Value.Has(playerEntity))
                    {
                        ref var networkComp = ref _networkPool.Value.Get(playerEntity);

                        if (networkComp.EntityKey == confirmDamageComp.SourceEntityKey)
                        {
                            //todo example damage view
                        }
                    }
                }
            }
        }
    }
}