using Swift_Solitaire.Scripts.Game;
using UnityEngine;

namespace Swift_Solitaire.Scripts.UI
{
    public class UndoBtn : CustomBtn {
        [SerializeField] CardManager cardManager;

        protected override void ClickedHandle() {
            cardManager.Undo();
        }
    }
}