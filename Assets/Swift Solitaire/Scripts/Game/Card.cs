using UnityEngine;

namespace Swift_Solitaire.Scripts.Game
{
    public class Card : MonoBehaviour
    {
        public int cardIndex { get; private set; }
        public Vector2Int coordinate { get; private set; }
        private CardManager cardManager;

        public void SetIndex(int cardIndex) {
            this.cardIndex = cardIndex;
        }

        public void SetCoordinate(Vector2Int coordinate) {
            this.coordinate = coordinate;
        }

        public void SetCardManager(CardManager cardManager) {
            this.cardManager = cardManager;
        }

        private void OnMouseDown() {
            cardManager.SelectCard(gameObject, coordinate);
        }
    }
}
