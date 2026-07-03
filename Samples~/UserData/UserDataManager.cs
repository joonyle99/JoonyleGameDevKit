using System;

namespace JoonyleGameDevKit
{
    [Serializable]
    public class UserData
    {
        public int coins;
    }

    public sealed class UserDataManager : UserDataManagerBase<UserData, UserDataManager>
    {
        public int Coins => Data.coins;

        public void AddCoins(int amount)
        {
            if (amount == 0) return;

            Data.coins = Math.Max(0, Data.coins + amount);
            Save();
        }

        protected override void OnDataLoaded(UserData data)
        {
            data.coins = Math.Max(0, data.coins); // 손상/조작된 세이브 대비
        }
    }
}
