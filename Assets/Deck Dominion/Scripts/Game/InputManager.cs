using UnityEngine;

namespace Deck_Dominion.Scripts.Game
{
    public class InputManager : MonoBehaviour {
        [SerializeField] float distanceThreshold = 0.5f;

        private Camera mainCamera;
        private GameObject selectedCard;
        private Vector3 offset;
        private bool isDragging = false;

        void Start() {
            mainCamera = Camera.main;
        }

        void Update() {
            if (!GameManager.Instance.IsPaused()) {
                if (Input.GetMouseButtonDown(0)) {
                    SelectCard();
                }

                if (isDragging && Input.GetMouseButton(0)) {
                    DragCard();
                }
            }
            else if (isDragging) {
                ReleaseCard();
            }

            if (isDragging && (Input.GetMouseButtonUp(0) || IsMouseOutsideScreen())) {
                ReleaseCard();
            }
        }

        private bool IsMouseOutsideScreen() {
            Vector2 mousePosition = Input.mousePosition;
            return mousePosition.x < 0 || mousePosition.x > Screen.width ||
                   mousePosition.y < 0 || mousePosition.y > Screen.height;
        }

        private void SelectCard() {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null) {
                GameObject hitObject = hit.collider.gameObject;
                Card card = hitObject.GetComponent<Card>();

                if (card != null) {
                    if (CardManager.Instance.MoveableCard(card.columnIndex, card.rowIndex) && card.isFaceUp) {
                        selectedCard = hitObject;
                        isDragging = true;

                        offset = mainCamera.ScreenToWorldPoint(Input.mousePosition) - selectedCard.transform.position;

                        selectedCard.transform.position = new Vector3(selectedCard.transform.position.x, selectedCard.transform.position.y, -20);

                        StartCoroutine(CardManager.Instance.SetSortingOrder(card, 100, 0f));
                    }
                }
            }
        }

        private void DragCard() {
            if (selectedCard != null) {
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                selectedCard.transform.position = new Vector3(mousePosition.x - offset.x, mousePosition.y - offset.y, selectedCard.transform.position.z);
            }
        }

        private void ReleaseCard() {
            if (selectedCard != null) {
                Card card = selectedCard.GetComponent<Card>();
                BoxCollider2D boxCollider = selectedCard.GetComponent<BoxCollider2D>();

                if (boxCollider != null) {
                    Vector2 boxSize = boxCollider.size;
                    Vector2 boxCenter = selectedCard.transform.position; 
                    Vector2 direction = Vector2.up; 

                    RaycastHit2D[] hits = Physics2D.BoxCastAll(boxCenter, boxSize, 0f, direction);

                    if (hits.Length == 0) {
                        CardManager.Instance.ReleaseCard(card);
                    }
                    else {
                        bool isMoved = false;
                        foreach (RaycastHit2D hit in hits) {
                            if (hit.collider != null) {
                                GameObject hitObject = hit.collider.gameObject;
                                Card hitCard = hitObject.GetComponent<Card>();
                                CardPlace cardPlace = hitObject.GetComponent<CardPlace>();

                                if (hitCard != null) {
                                    if (hitCard.columnIndex != card.columnIndex && hitCard.gameObject != selectedCard) {
                                        float xDistance = Mathf.Abs(selectedCard.transform.position.x - hitObject.transform.position.x);

                                        if (xDistance < distanceThreshold) {
                                            CardManager.Instance.ReleaseCard(card, hitCard.columnIndex);
                                            isMoved = true;
                                            break;
                                        }
                                    }
                                }

                                if (cardPlace != null) {
                                    int columnIndex = cardPlace.columnIndex;
                                    if (CardManager.Instance.cardStackList[columnIndex].Count == 0) {
                                        CardManager.Instance.ReleaseCard(card, cardPlace.columnIndex);
                                    }
                                }
                            }
                        }
                        if (!isMoved) {
                            CardManager.Instance.ReleaseCard(card);
                        }
                    }
                }
            }

            isDragging = false;
            selectedCard = null;
        }
    }
}