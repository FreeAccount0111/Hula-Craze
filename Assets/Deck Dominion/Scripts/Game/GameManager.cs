using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Deck_Dominion.Scripts.Game
{
    public enum GameState {
        Pause, Play
    }

    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<GameManager>();
                    if (_instance == null) {
                        _instance = new GameObject("GameManager").AddComponent<GameManager>();
                    }
                }

                return _instance;
            }
        }

        private GameState gameState = GameState.Play;

        public Action<bool> OnGameFinished;

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(gameObject);
            }
            else {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void ChangeScene(int index) {
            int maxIndexScene = SceneManager.sceneCountInBuildSettings;
            if (index >= 0 && index <= maxIndexScene + 1) {
                SceneManager.LoadScene(index);
                if (index >= 1) gameState = GameState.Play;
            }
        }

        public void SetGameState(GameState gameState) {
            this.gameState = gameState;
        }

        public bool IsPaused() {
            return gameState == GameState.Pause;
        }

        public void FinishGame(bool isWin) {
            OnGameFinished?.Invoke(isWin);
        }
    }
}