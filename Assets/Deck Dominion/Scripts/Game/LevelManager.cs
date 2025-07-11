using UnityEngine;

namespace Deck_Dominion.Scripts.Game
{
    public enum LevelType {
        Easy, Medium, Hard
    }

    public class LevelManager : MonoBehaviour {
        private static LevelManager _instance;
        public static LevelManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<LevelManager>();
                    if (_instance == null) {
                        _instance = new GameObject("LevelManager").AddComponent<LevelManager>();
                    }
                }

                return _instance;
            }
        }

        public LevelType levelType { get; private set; }

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(gameObject);
            }
            else {
                _instance = this;
                DontDestroyOnLoad(gameObject);

                levelType = LevelType.Easy;
            }
        }

        public void SetLevelType(LevelType type) {
            levelType = type;
        }
    }
}