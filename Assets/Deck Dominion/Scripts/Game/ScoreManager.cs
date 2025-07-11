using TMPro;
using UnityEngine;

namespace Deck_Dominion.Scripts.Game
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] TMP_Text scoreText;
        [SerializeField] int startScore;
        [SerializeField] int inscreaseScoreStep = 1000;
        [SerializeField] int decreaseScoreStep = 50;
        public int Score { get; private set; }

        private void Awake() {
            Score = startScore;
        }

        private void Start() {
            ShowScore();
        }

        private void ShowScore() {
            scoreText.text = Score.ToString();
        }

        public void IncreaseScore() {
            Score += inscreaseScoreStep;
            ShowScore();
        }

        public void DescreaseScore() {
            Score -= decreaseScoreStep;
            if (Score < 0) Score = 0;
            ShowScore();

            if (Score <= 0) {
                GameManager.Instance.FinishGame(false);
            }
        }
    }
}
