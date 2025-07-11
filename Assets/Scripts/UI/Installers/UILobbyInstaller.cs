using System.Collections;
using Gameplay.Interface;
using Gameplay.Manager;
using UI.Interfaces;
using UI.Models;
using UI.Presenters;
using UnityEngine;

namespace UI.Installers
{
   public class UILobbyInstaller : MonoBehaviour
   {
      [SerializeField] private GameObject lobbyView;
      private UserModel _userModel;
      private UILobbyPresenter _uiLobbyPresenter;

      IEnumerator Start()
      {
         yield return new WaitUntil(() => UserManager.Instance != null);
            
         _userModel = UserManager.Instance.GetUserModel(0);
         _uiLobbyPresenter = new UILobbyPresenter(lobbyView.GetComponent<ILobbyView>(), _userModel,LoadSceneManager.Instance.GetComponent<ILoadScene>());
         _uiLobbyPresenter.Initialized();
      }

      private void OnDestroy()
      {
         _uiLobbyPresenter.Unsubscribe();
      }
   }
}
