using Data;
using UI.Events;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.Models
{
    public class UserModel
    {
        private readonly UserData _userData;
        private int _indexGame;
        public UserData UserData => _userData;
        public DataBet DataBet => _userData.dataBets[_indexGame];

        public UserModel(UserData data, int indexGame)
        {
            _userData = data;
            _indexGame = indexGame;
        }

        public void IncreaseBet()
        {
            _userData.dataBets[_indexGame].currentBet += 100;

            UpdateValue();
        }

        public void IncreaseLine()
        {
            if (_userData.dataBets[_indexGame].currentLine + 1 <= 24)
                _userData.dataBets[_indexGame].currentLine += 1;

            UpdateValue();
        }

        public void ReductionBet()
        {
            if (_userData.dataBets[_indexGame].currentBet - 100 > 0)
                _userData.dataBets[_indexGame].currentBet -= 100;

            UpdateValue();
        }

        public void ReductionLine()
        {
            if (_userData.dataBets[_indexGame].currentLine - 1 > 0)
                _userData.dataBets[_indexGame].currentLine -= 1;

            UpdateValue();
        }

        public void CheckResult(int coin,int spin)
        {
            _userData.currentCoin += coin;
            _userData.dataBets[_indexGame].freeSpins += spin;
            UpdateValue();
        }

        public void MaxBet()
        {
            for (int i = 10; i >= 0; i--)
            {
                _userData.dataBets[_indexGame].currentBet = ((_userData.currentCoin / _userData.dataBets[_indexGame].currentLine) / 100) * 100;
                if (_userData.dataBets[_indexGame].currentBet * _userData.dataBets[_indexGame].currentLine <= _userData.currentCoin)
                    break;
            }

            UpdateValue();
        }

        public void Spin()
        {
            if (_userData.dataBets[_indexGame].freeSpins > 0)
            {
                _userData.dataBets[_indexGame].freeSpins -= 1;
                UIBetEvent.RaiseSpinSuccess(true);
                UpdateValue();
                return;
            }
            
            if (_userData.currentCoin >= _userData.dataBets[_indexGame].currentBet * _userData.dataBets[_indexGame].currentLine)
            {
                _userData.currentCoin -= _userData.dataBets[_indexGame].currentBet * _userData.dataBets[_indexGame].currentLine;
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
           UIBetEvent.RaiseUpdateData();
        }
    }
}
