namespace JoonyleGameDevKit
{
    public enum BgmType
    {
        Title = 1,

        OutGame = 2,

        InGame = 3,
    }

    public enum SfxType
    {
        // UI
        UI_Click = 1,
        
        // Effect 1
        Attack = 50,

        // Effect 2
        Dust = 100,
    }

    public sealed class SoundManager : SoundManagerBase<BgmType, SfxType, SoundManager>
    {

    }
}
