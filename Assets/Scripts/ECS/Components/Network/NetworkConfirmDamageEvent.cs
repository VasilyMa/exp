using Leopotam.EcsLite;
using MemoryPack;
using Statement;

namespace Client 
{
    [MemoryPackable]
    public partial struct NetworkConfirmDamageEvent : IRequestable
    {
        public float DamageValue;
        public DamageType DamageType;
        public string SourceEntityKey;
        public string TargetEntityKey;

        public void Request(EcsWorld world)
        {
            var state = BattleState.Instance;
             
            if (state.TryGetEntity(TargetEntityKey, out int targetEntity))
            {
                if (!world.GetPool<ConfirmDamageUpdateEvent>().Has(targetEntity))
                {
                    world.GetPool<ConfirmDamageUpdateEvent>().Add(targetEntity);
                }

                ref var confirmDamageComp = ref world.GetPool<ConfirmDamageUpdateEvent>().Get(targetEntity);
                confirmDamageComp.SourceEntityKey = SourceEntityKey;
                confirmDamageComp.DamageValue = DamageValue;
                confirmDamageComp.DamageType = DamageType;
            }
        }
    }
}