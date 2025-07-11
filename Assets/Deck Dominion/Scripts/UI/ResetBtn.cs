using Deck_Dominion.Scripts.Game;

namespace Deck_Dominion.Scripts.UI
{
    public class ResetBtn : ChangeSceneBtn
    {
        protected override void ClickedHandle() {
            if (GameManager.Instance.IsPaused()) return;
            base.ClickedHandle();
        }
    }
}
