using Deck_Dominion.Scripts.Game;
using UnityEngine;

namespace Deck_Dominion.Scripts.UI
{
    public class SelectLevelBtn : ChangeSceneBtn
    {
        [SerializeField] LevelType levelType;

        protected override void ClickedHandle() {
            LevelManager.Instance.SetLevelType(levelType);

            base.ClickedHandle();
        }
    }
}
