using Data;
using UI.Events;
using UnityEngine;

namespace UI.Models
{
    public class UserModel
    {
        private readonly UserData _userData;
        public UserData UserData => _userData;

        public UserModel(UserData data)
        {
            _userData = data;
        }

        public void IncreaseBet()
        {
            _userData.currentBet += 100;

            UpdateValue();
        }

        public void IncreaseLine()
        {
            if (_userData.currentLine + 1 <= 24)
                _userData.currentLine += 1;

            UpdateValue();
        }

        public void ReductionBet()
        {
            if (_userData.currentBet - 100 > 0)
                _userData.currentBet -= 100;

            UpdateValue();
        }

        public void ReductionLine()
        {
            if (_userData.currentLine - 1 > 0)
                _userData.currentLine -= 1;

            UpdateValue();
        }

        public void CheckResult(int coin,int spin)
        {
            _userData.currentCoin += coin;
            _userData.freeSpins += spin;
            UpdateValue();
        }

        public void MaxBet()
        {
            for (int i = 10; i >= 0; i--)
            {
                _userData.currentBet = ((_userData.currentCoin / _userData.currentLine) / 100) * 100;
                if (_userData.currentBet * _userData.currentLine <= _userData.currentCoin)
                    break;
            }

            UpdateValue();
        }

        public void Spin()
        {
            if (_userData.currentCoin >= _userData.currentBet * _userData.currentLine)
            {
                _userData.currentCoin -= _userData.currentBet * _userData.currentLine;
                UIBetEvent.RaiseSpinSuccess(true);
            }
            else
            {
                UIBetEvent.RaiseSpinSuccess(false);
            }
            
            UpdateValue();
        }

        private void UpdateValue()
        {
           UIBetEvent.RaiseUpdateData(_userData);
        }
    }
}
