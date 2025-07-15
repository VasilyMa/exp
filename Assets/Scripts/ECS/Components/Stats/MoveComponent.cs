using Leopotam.EcsLite;
using UnityEngine;

namespace Client 
{
    struct MoveComponent : IStat
    {
        public float BaseValue;
        private float maxValue;
        private float baseValue;
        private float currentValue;
        public float GetMaxValue { get => maxValue; }
        public float GetCurrentValue { get => currentValue; }

        public void Init(EcsWorld world, int entity)
        {
            ref var moveComp = ref world.GetPool<MoveComponent>().Add(entity);
            moveComp.baseValue = BaseValue;
            moveComp.currentValue = BaseValue;
            moveComp.maxValue = BaseValue;
        }

        public void Add(float value)
        {
            currentValue += value;

            ClampCurrentValue();
        }

        public void SetCurrent(float value)
        {
            currentValue = value;
        }

        public void Sub(float value)
        {
            currentValue -= value;

            ClampCurrentValue();
        }

        void ClampCurrentValue()
        {
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        }


    }
}