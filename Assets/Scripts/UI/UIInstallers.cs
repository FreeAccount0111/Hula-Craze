using System;
using System.Collections;
using System.Collections.Generic;
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

        private BetModel _betModel;
        private UIBetPresenter _uiBetPresenter;

        IEnumerator Start()
        {
            yield return new WaitUntil(() => UserManager.Instance != null);
            _betModel = new BetModel(UserManager.Instance.UserData);
            _uiBetPresenter = new UIBetPresenter(betView.GetComponent<IBetView>(), _betModel);
        }

        private void OnDestroy()
        {
            _uiBetPresenter.Unsubscribe();
        }
    }
}
