using System.Collections;
using Gameplay.Controller;
using Gameplay.Manager;
using UI.Interfaces;
using UI.Models;
using UI.Presenters;
using UnityEngine;

namespace UI.Installers
{
    public class UISlotInstaller : MonoBehaviour
    {
        [SerializeField] private int indexGame;
        [SerializeField] private GameObject betView;
        [SerializeField] private LineController lineCtrl;
        [SerializeField] private GameObject winView;

        private UserModel _userModel;

        private UIWinPresenter _uiWinPresenter;
        private UIBetPresenter _uiBetPresenter;

        IEnumerator Start()
        {
            yield return new WaitUntil(() => UserManager.Instance != null);
            
            _userModel = UserManager.Instance.GetUserModel(indexGame);
            _uiBetPresenter = new UIBetPresenter(betView.GetComponent<IBetView>(), lineCtrl.GetComponent<ILineView>(), _userModel);
            _uiWinPresenter = new UIWinPresenter(winView.GetComponent<IWinView>(),_userModel);
            
            _uiBetPresenter.Initialized();
        }

        private void OnDestroy()
        {
            _uiBetPresenter.Unsubscribe();
            _uiWinPresenter.Unsubscribe();
        }
    }
}
