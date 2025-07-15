using System.Collections.Generic;

namespace Client 
{
    struct TakeDamageEvent 
    {
        public List<TakeDamageData> DamageData;
    }

    struct TakeDamageData
    {
        public float Value;
        public DamageType Type;
        public string SourceEntityKey;
    }
}