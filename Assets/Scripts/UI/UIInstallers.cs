using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Controller;
using Gameplay.Manager;
using UI.Interfaces;
using UI.Models;
using UI.Presenters;
using UI.Views;
using UnityEngine;

namespace UI
{
    public class UIInstallers : MonoBehaviour
    {
        [SerializeField] private BetView betView;
        [SerializeField] private LineController lineCtrl;
        [SerializeField] private WinView winView;

        private UserModel _userModel;

        private UIWinPresenter _uiWinPresenter;
        private UIBetPresenter _uiBetPresenter;

        IEnumerator Start()
        {
            yield return new WaitUntil(() => UserManager.Instance != null);
            
            _userModel = UserManager.Instance.userModel;
            _uiBetPresenter = new UIBetPresenter(betView.GetComponent<IBetView>(), lineCtrl.GetComponent<ILineView>(), _userModel);
            _uiWinPresenter = new UIWinPresenter(winView.GetComponent<IWinView>(),_userModel);
            
            _uiBetPresenter.Initialized();
        }

        private void OnDestroy()
        {
            _uiBetPresenter.Unsubscribe();
        }
    }
}
