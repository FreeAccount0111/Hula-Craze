using UnityEngine;

namespace Swift_Solitaire.Scripts.Game
{
    public class GridManager : MonoBehaviour
    {
        public int rows = 4;
        public int columns = 13;
        [SerializeField] float rowSize;
        [SerializeField] float columnSize;
        [SerializeField] Vector2 offset;
        [SerializeField] float spacing;

        [SerializeField] GameObject gridFramePrefab;

        private void Start() {
            InitGridBackground();
        }

        private void InitGridBackground() {
            for (int y = 0; y < rows; y++) {
                for (int x = 0; x < columns; x++) {
                    Vector2 pos = GetGridPosition(new Vector2Int(x, y));
                    GameObject cardObj = Instantiate(gridFramePrefab, pos, Quaternion.identity, transform);
                    SpriteRenderer spriteRenderer = cardObj.GetComponent<SpriteRenderer>();
                    float cardWidth = spriteRenderer.bounds.size.x;
                    float cellWidth = columnSize - spacing * 2;
                    float targetScale = cellWidth / cardWidth;
                    cardObj.transform.localScale = new Vector3(targetScale, targetScale, targetScale);
                }
            }
        }

        public Vector2 GetGridPosition(Vector2Int coordinate) {
            return new Vector2(
                transform.position.x + coordinate.x * columnSize + offset.x,
                transform.position.y - coordinate.y * rowSize - offset.y
            );
        }

        public float GetTargetScale(Sprite sprite) {
            float spriteWidth = sprite.bounds.size.x;
            return columnSize / spriteWidth;
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;

            for (int y = 0; y < rows; y++) {
                for (int x = 0; x < columns; x++) {
                    Vector2 pos = GetGridPosition(new Vector2Int(x, y));
                    Gizmos.DrawWireCube(pos, new Vector3(columnSize, rowSize, 0.1f));
                }
            }
        }
    }
}
