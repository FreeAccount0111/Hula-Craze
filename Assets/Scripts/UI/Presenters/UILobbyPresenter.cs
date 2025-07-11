using Gameplay.Interface;
using UI.Component;
using UI.Events;
using UI.Interfaces;
using UI.Models;

namespace UI.Presenters
{
    public class UILobbyPresenter
    {
        private ILobbyView _view;
        private ILoadScene _load;
        private UserModel _model;

        public UILobbyPresenter(ILobbyView view, UserModel model,ILoadScene load)
        {
            _view = view;
            _load = load;
            _model = model;

            UILobbyEvent.OnLoadGame += LoadGame;
        }

        public void Initialized()
        {
            UpdateValueData();
        }

        private void UpdateValueData()
        {
            _view.UpdateCoinText(_model.UserData.currentCoin);
            _view.UpdateLevelText(_model.UserData.currentLevel);
            _view.UpdateLevelExp(_model.UserData.currentExp / 10f * _model.UserData.currentLevel);
        }

        public void Unsubscribe()
        {
            UILobbyEvent.OnLoadGame -= LoadGame;
        }

        private void LoadGame(string nameScene) => _load.LoadScene(nameScene);
    }
}
