using System;
using System.Threading.Tasks;
using Gameplay.Events;
using UI.Interfaces;
using UI.Models;
using Cysharp.Threading.Tasks;
using Gameplay.Manager;
using UnityEngine;

namespace UI.Presenters
{
    public class UIWinPresenter
    {
        private IWinView _view;
        private UserModel _model;

        public UIWinPresenter(IWinView view, UserModel model)
        {
            _view = view;
            _model = model;

            GameEvent.OnPayout += Payout;
        }

        public void Unsubscribe()
        {
            GameEvent.OnPayout -= Payout;
        }

        private void Payout(int price, int spinFree)
        {
            _view.UpdateWinText(price);
            ShowResult(price, spinFree).Forget();
        }

        private async UniTask ShowResult(int price, int spinFree)
        {
            try
            {
                _model.CheckResult(price, spinFree);
                SaveData();

                int ratio = price / _model.DataBet.currentBet;
                
                if (ratio >= 150)
                {
                    _view.ShowBacGround();
                    await UniTask.Delay(500);
                    
                    _view.ShowSuperWin(price);
                    await UniTask.Delay(3000);
                    _view.HideSuperWin();
                    await UniTask.Delay(500);
                }
                else if (ratio >= 65)
                {
                    _view.ShowBacGround();
                    await UniTask.Delay(500);

                    _view.ShowMegaWin(price);
                    await UniTask.Delay(3000);
                    _view.HideMegaWin();
                    await UniTask.Delay(500);
                }
                else if (ratio >= 30)
                {
                    _view.ShowBacGround();
                    await UniTask.Delay(500);

                    _view.ShowBigWin(price);
                    await UniTask.Delay(3000);
                    _view.HideBigWin();
                    await UniTask.Delay(500);
                }

                if (spinFree > 0)
                {
                    _view.ShowSpinFree(spinFree);
                    await UniTask.Delay(3000);
                    _view.HideSpinFree();
                    await UniTask.Delay(500);
                }
                
                _view.HideBackGround();
                GameEvent.RaiseResetRoll();
            }
            catch (Exception e)
            {
                Debug.LogError($"Payout error: {e}");
            }
        }
        private void SaveData() => UserManager.Instance.SaveData();
    }
}
