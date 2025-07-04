using Data;
using UI.Events;
using UnityEngine;

namespace UI.Models
{
    public class BetModel
    {
        private UserData _userData;
        private int _indexBet;
        private int _currentBet;

        public BetModel(UserData data)
        {
            _userData = data;

            _indexBet = 1;
            _currentBet = _indexBet * 1000;
        }

        public void IncreaseBet()
        {
            _indexBet = _indexBet + 1 < 10 ? _indexBet + 1 : 10;
            _currentBet = _indexBet * 1000;
            
            UIBetEvent.RaiseUpdateBet(_currentBet);
        }

        public void ReductionBet()
        {
            _indexBet = _indexBet - 1 > 0 ? _indexBet - 1 : 0;
            _currentBet = _indexBet * 1000;
            
            UIBetEvent.RaiseUpdateBet(_currentBet);
        }

        public void MaxBet()
        {
            for (int i = 10; i >= 0; i--)
            {
                _indexBet = i;
                _currentBet = _indexBet * 1000;
                if (_currentBet <= _userData.CurrentCoin)
                    break;
            }
            
            UIBetEvent.RaiseUpdateBet(_currentBet);
        }

        public void Spin()
        {
            if (_userData.CurrentCoin >= _currentBet)
            {
                _userData.ReductionCoin(_currentBet);
                UIBetEvent.RaiseSpinSuccess(true);
            }
            else
            {
                UIBetEvent.RaiseSpinSuccess(false);
            }
        }
    }
}
