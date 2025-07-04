using System.Threading;
using Gameplay.Events;
using UI.Events;
using UI.Interfaces;
using UI.Models;

namespace UI.Presenters
{
    public class UIBetPresenter
    {
        private IBetView _view;
        private BetModel _model;

        public UIBetPresenter(IBetView view, BetModel model)
        {
            _view = view;
            _model = model;

            UIBetEvent.OnIncreaseBet += IncreaseBet;
            UIBetEvent.OnReductionBet += ReductionBet;
            UIBetEvent.OnMaxBet += MaxBet;
            UIBetEvent.OnClickSpin += ClickSpin;
            UIBetEvent.OnSpinSuccess += SpinSuccess;

            UIBetEvent.OnUpdateBet += UpdateBet;
        }

        public void Unsubscribe()
        {
            UIBetEvent.OnIncreaseBet -= IncreaseBet;
            UIBetEvent.OnReductionBet -= ReductionBet;
            UIBetEvent.OnMaxBet -= MaxBet;
            UIBetEvent.OnClickSpin -= ClickSpin;
            UIBetEvent.OnSpinSuccess -= SpinSuccess;
            
            UIBetEvent.OnUpdateBet -= UpdateBet;
        }

        private void IncreaseBet()
        {
            _model.IncreaseBet();
        }

        private void ReductionBet()
        {
            _model.ReductionBet();
        }

        private void UpdateBet(int amount)
        {
            _view.UpdateBetText(amount);
        }

        private void MaxBet()
        {
            _model.MaxBet();
        }

        private void ClickSpin()
        {
           _model.Spin();
        }

        private void SpinSuccess(bool en)
        {
            if (en)
            {
                GameEvent.RaiseSpin();
                _view.UpdateNotification("Good luck!!!");
            }
            else
            {
                _view.UpdateNotification("You don't have enough money.");
            }
        }
    }
}
