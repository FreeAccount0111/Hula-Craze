using Deck_Dominion.Scripts.Game;
using UnityEngine;

namespace Deck_Dominion.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Dialog : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        protected virtual void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show() {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            GameManager.Instance.SetGameState(GameState.Pause);
        }

        public virtual void Hide() {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            GameManager.Instance.SetGameState(GameState.Play);
        }
    }
}
