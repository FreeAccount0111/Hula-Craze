using UnityEngine;

namespace Deck_Dominion.Scripts.Game
{
    public class DealCard : MonoBehaviour
    {
        private bool isFinished = false;
        private void Awake() {
            GameManager.Instance.OnGameFinished += FinishHandle;
        }

        private void OnMouseDown() {
            if (GameManager.Instance.IsPaused() || isFinished) return;

            StartCoroutine(CardManager.Instance.DealCards(CardManager.nextDealCardCount, true));
        }

        private void FinishHandle(bool isWin) {
            isFinished = true;
        }
    }
}
