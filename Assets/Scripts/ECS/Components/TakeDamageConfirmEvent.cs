 
namespace Client {
    struct TakeDamageConfirmEvent 
    {
        public float DamageValue;
        public DamageType DamageType;
        public string SourceEntityKey;
        public string TargetEntityKey;
    }
}