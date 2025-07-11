using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Deck_Dominion.Scripts.Game
{
    public class FinishGame : MonoBehaviour
    {
        [SerializeField] ScoreManager scoreManager;
        [SerializeField] Image finishImage;
        [SerializeField] Sprite winFrame;
        [SerializeField] Sprite loseFrame;
        [SerializeField] TMP_Text scoreText;
        [SerializeField] TMP_Text bestScoreText;

        CanvasGroup canvasGroup;
        bool isFinished = false;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();

            GameManager.Instance.OnGameFinished += FinishHandle;
        }

        private void FinishHandle(bool isWin) {
            if (isFinished) return;
            isFinished = true;

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            int score = 0;
            int bestScore = PlayerPrefs.GetInt("BestScore");
            if (isWin) {
                score = scoreManager.Score;
                if (score > bestScore) {
                    bestScore = score;
                    PlayerPrefs.SetInt("BestScore", bestScore);
                }
            }


            scoreText.text = score.ToString();
            bestScoreText.text = bestScore.ToString();

            finishImage.sprite = isWin ? winFrame : loseFrame;
            finishImage.SetNativeSize();
        }
    }
}
