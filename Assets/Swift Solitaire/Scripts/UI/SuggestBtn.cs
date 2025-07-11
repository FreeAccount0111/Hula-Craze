using Swift_Solitaire.Scripts.Game;
using UnityEngine;

namespace Swift_Solitaire.Scripts.UI
{
    public class SuggestBtn : CustomBtn {
        [SerializeField] CardManager cardManager;

        protected override void ClickedHandle() {
            cardManager.DisplaySuggestion(true);
        }
    }
}