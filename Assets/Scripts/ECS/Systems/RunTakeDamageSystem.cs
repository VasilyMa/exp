using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunTakeDamageSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageEvent, HealthComponent, NetworkEntityComponent>, Exc<InvicibleComponent>> _filter = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<TakeDamageEvent> _takeDamagePool = default;
        readonly EcsPoolInject<HealthUpdateEvent> _healtUpdateEventPool = default;
        readonly EcsPoolInject<NetworkEntityComponent> _networkEntityPool = default;
        readonly EcsPoolInject<TakeDamageConfirmEvent> _confirmEventPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var healthComp = ref _healthPool.Value.Get(entity);
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                ref var networkEntityComp = ref _networkEntityPool.Value.Get(entity);


                foreach (var damageData in takeDamageComp.DamageData)
                {
                    healthComp.Sub(damageData.Value);

                    ref var confirmDamageComp = ref _confirmEventPool.Value.Add(_world.Value.NewEntity());
                    confirmDamageComp.DamageValue = damageData.Value;
                    confirmDamageComp.DamageType = damageData.Type;
                    confirmDamageComp.SourceEntityKey = damageData.SourceEntityKey;
                    confirmDamageComp.TargetEntityKey = networkEntityComp.EntityKey;

                    if (healthComp.GetCurrentValue <= 0)
                    {
                        break;
                    }
                }

                ref var healthUpdateComp = ref _healtUpdateEventPool.Value.Add(_world.Value.NewEntity());
                healthUpdateComp.CurrentValue = healthComp.GetCurrentValue;
                healthUpdateComp.EntityKey = networkEntityComp.EntityKey;
            }
        }
    }
}