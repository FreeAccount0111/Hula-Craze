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
            Debug.Log(price / _model.UserData.currentBet);
            ShowResult(price, spinFree).Forget();
        }

        private async UniTask ShowResult(int price, int spinFree)
        {
            try
            {
                _model.CheckResult(price, spinFree);
                SaveData();

                int ratio = price / _model.UserData.currentBet;
                
                if (ratio >= 50)
                {
                    _view.ShowBacGround();
                    await UniTask.Delay(500);
                    
                    _view.ShowSuperWin(price);
                    await UniTask.Delay(1000);
                }
                else if (ratio >= 20)
                {
                    _view.ShowBacGround();
                    await UniTask.Delay(500);

                    _view.ShowMegaWin(price);
                    await UniTask.Delay(1000);
                }
                else if (ratio >= 0)
                {
                    _view.ShowBacGround();
                    await UniTask.Delay(500);

                    _view.ShowBigWin(price);
                    await UniTask.Delay(1000);
                }

                if (spinFree > 0)
                {
                    _view.ShowSpinFree(spinFree);
                    await UniTask.Delay(1000);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Payout error: {e}");
            }
        }
        private void SaveData() => UserManager.Instance.SaveData();
    }
}
