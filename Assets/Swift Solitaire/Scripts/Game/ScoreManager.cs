using Swift_Solitaire.Scripts.UI;
using TMPro;
using UnityEngine;
using TimeCounter = Deck_Dominion.Scripts.UI.TimeCounter;

namespace Swift_Solitaire.Scripts.Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ScoreManager : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        [SerializeField] TimeCounter timeCounter;
        [SerializeField] int scoreTimeFactor;
        [SerializeField] CardManager cardManager;
        [SerializeField] int scoreMoveFactor;

        [SerializeField] TMP_Text scoreTMP;
        [SerializeField] TMP_Text bestScoreTMP;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
            GameManager.Instance.OnGameFinished += ShowScore;
        }

        private void ShowScore() {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;

            int score = (int)(timeCounter.GetTimeRemaining() * scoreTimeFactor) - (cardManager.GetMoveCount() * scoreMoveFactor);
            if (score < 0) score = 0;

            int bestScore = PlayerPrefs.GetInt("BestScore");
            if (score > bestScore) {
                bestScore = score;
                PlayerPrefs.SetInt("BestScore", bestScore);
            }

            scoreTMP.text = score.ToString();
            bestScoreTMP.text = bestScore.ToString();
        }
    }
}
