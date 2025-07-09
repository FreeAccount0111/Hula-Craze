using UI.Interfaces;
using UI.Models;
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
        }
    }
}
