using System.Threading;
using Data;
using Gameplay.Controller;
using Gameplay.Events;
using Gameplay.Manager;
using UI.Events;
using UI.Interfaces;
using UI.Models;
using UI.Views;

namespace UI.Presenters
{
    public class UIBetPresenter
    {
        private IBetView _viewBet;
        private ILineView _viewLine;
        private UserModel _model;

        public UIBetPresenter(IBetView viewBet, ILineView viewLine, UserModel model)
        {
            _viewBet = viewBet;
            _viewLine = viewLine;
            _model = model;

            UIBetEvent.OnIncreaseBet += IncreaseBet;
            UIBetEvent.OnReductionBet += ReductionBet;
            UIBetEvent.OnIncreaseLine += IncreaseLine;
            UIBetEvent.OnReductionLine += ReductionLine;
            
            UIBetEvent.OnMaxBet += MaxBet;
            UIBetEvent.OnClickSpin += ClickSpin;
            UIBetEvent.OnSpinSuccess += SpinSuccess;

            UIBetEvent.OnUpdateData += UpdateData;
        }
        public void Unsubscribe()
        {
            UIBetEvent.OnIncreaseBet -= IncreaseBet;
            UIBetEvent.OnReductionBet -= ReductionBet;
            UIBetEvent.OnIncreaseLine -= IncreaseLine;
            UIBetEvent.OnReductionLine -= ReductionLine;
            
            UIBetEvent.OnMaxBet -= MaxBet;
            UIBetEvent.OnClickSpin -= ClickSpin;
            UIBetEvent.OnSpinSuccess -= SpinSuccess;
            
            UIBetEvent.OnUpdateData -= UpdateData;
        }

        public void Initialized() => UpdateData(_model.UserData);
        private void IncreaseBet() => _model.IncreaseBet();
        private void ReductionBet() => _model.ReductionBet();
        private void IncreaseLine()
        {
            _viewLine.ShowLines();
            _model.IncreaseLine();
        }
        private void ReductionLine()
        {
            _viewLine.ShowLines();
            _model.ReductionLine();
        }
        private void UpdateData(UserData data)
        {
            _viewBet.UpdateCreditText(data.currentCoin);
            _viewBet.UpdateBetText(data.currentBet);
            _viewBet.UpdateLineText(data.currentLine);
            _viewLine.UpdateLine(data.currentLine);
            _viewBet.UpdateTotalText(data.currentBet * data.currentLine);
            _viewBet.UpdateSpinText(data.freeSpins);

            _viewBet.UpdateSpin(_model.UserData.freeSpins > 0);
        }
        private void MaxBet() => _model.MaxBet();
        private void ClickSpin() => _model.Spin();
        private void SpinSuccess(bool en)
        {
            if (en)
            {
                GameEvent.RaiseSpin();
                _viewLine.HideLines();
                _viewBet.UpdateNotification("Good luck!!!");
            }
            else
                _viewBet.UpdateNotification("You don't have enough money.");
            
            SaveData();
        }
        private void SaveData() => UserManager.Instance.SaveData();
    }
}
