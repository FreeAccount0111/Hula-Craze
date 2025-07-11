using Deck_Dominion.Scripts.Game;
using UnityEngine;

namespace Deck_Dominion.Scripts.UI
{
    public class ChangeSceneBtn : CustomBtn {
        [SerializeField] protected int sceneIndex;

        protected override void ClickedHandle() {
            GameManager.Instance.ChangeScene(sceneIndex);
        }
    }
}