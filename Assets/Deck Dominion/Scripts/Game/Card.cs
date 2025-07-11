using UnityEngine;

namespace Deck_Dominion.Scripts.Game
{
    public class Card : MonoBehaviour
    {
        public int cardIndex { get; private set; }
        public int columnIndex { get; private set; }
        public int rowIndex { get; private set; }
        public bool isFaceUp { get; private set; }

        private Sprite sprite;

        private SpriteRenderer spriteRenderer;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            isFaceUp = false;
        }

        public void SetIndex(int cardIndex) {
            this.cardIndex = cardIndex;
        }

        public void SetColumnIndex(int columnIndex) {
            this.columnIndex = columnIndex;
        }

        public void SetRowIndex(int rowIndex) {
            this.rowIndex = rowIndex;
        }

        public void SetSprite(Sprite sprite) {
            this.sprite = sprite;
        }

        public void SetSortingOrder(int sortOrder) {
            spriteRenderer.sortingOrder = sortOrder;
        }

        public void FaceUp() {
            spriteRenderer.sprite = sprite;
            isFaceUp = true;
        }
    }
}
