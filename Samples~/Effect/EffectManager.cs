namespace JoonyleGameDevKit
{
    public enum VfxType
    {
        Attack = 1,
        Jump = 2,

        Dust = 50,
        Explosion = 51,
    }

    public sealed class EffectManager : EffectManagerBase<VfxType, EffectManager>
    {
        
    }
}
