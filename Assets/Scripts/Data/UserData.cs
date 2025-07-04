using System;

namespace Data
{
    [Serializable]
    public class UserData
    {
        private int _currentCoin;
        
        public int CurrentCoin
        {
            get => _currentCoin;
            set => _currentCoin = value;
        }

        public UserData()
        {
            _currentCoin = 5000;
        }

        public void IncreaseCoin(int amount)
        {
            _currentCoin += amount;
        }

        public void ReductionCoin(int amount)
        {
            _currentCoin -= amount;
        }
    }
}
