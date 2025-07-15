using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunSendConfirmDamageSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageConfirmEvent>> _filter = default;
        readonly EcsPoolInject<TakeDamageConfirmEvent> _takeDamageConfirmPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var takeDamageConfirmComp = ref _takeDamageConfirmPool.Value.Get(entity);

                NetworkConfirmDamageEvent networkConfirmDamageEvent = new NetworkConfirmDamageEvent()
                {
                    DamageType = takeDamageConfirmComp.DamageType,
                    DamageValue = takeDamageConfirmComp.DamageValue,
                    SourceEntityKey = takeDamageConfirmComp.SourceEntityKey,
                    TargetEntityKey = takeDamageConfirmComp.TargetEntityKey,
                };

                PhotonRunHandler.Instance.SendRequestConfirmDamageRPC(MemoryPack.MemoryPackSerializer.Serialize(networkConfirmDamageEvent));
            }
        }
    }
}