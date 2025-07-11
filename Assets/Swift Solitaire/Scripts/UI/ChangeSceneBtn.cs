using Swift_Solitaire.Scripts.Game;
using UnityEngine;
using GameManager = Deck_Dominion.Scripts.Game.GameManager;

namespace Swift_Solitaire.Scripts.UI
{
    public class ChangeSceneBtn : CustomBtn {
        [SerializeField] int sceneIndex;

        protected override void ClickedHandle() {
            GameManager.Instance.ChangeScene(sceneIndex);
        }
    }
}