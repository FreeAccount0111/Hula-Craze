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
            UIBetEvent.OnShowHtp += ShowHtp;
            UIBetEvent.OnHideHtp += HideHtp;

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
            UIBetEvent.OnShowHtp -= ShowHtp;
            UIBetEvent.OnHideHtp -= HideHtp;
            
            UIBetEvent.OnUpdateData -= UpdateData;
        }

        public void Initialized() => UpdateData();

        private void IncreaseBet()
        {
            _model.IncreaseBet();
            SlotMachineController.Instance.StopShowWin();
        }

        private void ReductionBet()
        {
            _model.ReductionBet();
            SlotMachineController.Instance.StopShowWin();
        } 
        private void IncreaseLine()
        {
            _viewLine.ShowLines();
            _model.IncreaseLine();
            SlotMachineController.Instance.StopShowWin();
            
        }
        private void ReductionLine()
        {
            _viewLine.ShowLines();
            _model.ReductionLine();
            SlotMachineController.Instance.StopShowWin();
        }

        private void ShowHtp()
        {
            _viewBet.ShowHtp();
        }

        private void HideHtp()
        {
            _viewBet.HideHtp();
        }
        private void UpdateData()
        {
            _viewBet.UpdateCreditText(_model.UserData.currentCoin);
            _viewBet.UpdateBetText(_model.DataBet.currentBet);
            _viewBet.UpdateLineText(_model.DataBet.currentLine);
            _viewLine.UpdateLine(_model.DataBet.currentLine);
            _viewBet.UpdateTotalText(_model.DataBet.currentBet * _model.DataBet.currentLine);
            _viewBet.UpdateSpinText(_model.DataBet.freeSpins);

            _viewBet.UpdateSpin(_model.DataBet.freeSpins > 0);
        }
        private void MaxBet() => _model.MaxBet();
        private void ClickSpin()
        {
            if(SlotMachineController.Instance.CanRoll)
                _model.Spin();
        }
        private void SpinSuccess(bool en)
        {
            if (en)
            {
                GameEvent.RaiseSpin(_model.DataBet.currentBet,_model.DataBet.currentLine);
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
