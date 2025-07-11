using System.Collections;
using System.Collections.Generic;
using Deck_Dominion.Scripts.UI;
using UnityEngine;

namespace Deck_Dominion.Scripts.Game
{
    public class CardManager : MonoBehaviour
    {
        private static CardManager instance;
        public static CardManager Instance {
            get { return instance; }
        }

        [SerializeField] List<Sprite> cardSprites;
        [SerializeField] GameObject cardPrefab;
        [SerializeField] float spacingOfDownCard;
        [SerializeField] float spacingOfUpCard;
        [SerializeField] float moveTime = 1.5f;
        [SerializeField] float dealSpaceTime = 0.25f;

        [SerializeField] CardTable cardTable;
        [SerializeField] TimeCounter timeCounter;
        [SerializeField] ScoreManager scoreManager;

        [SerializeField] float minLimitToReSpacing = 4f;

        [HideInInspector] public List<List<Card>> cardStackList = new List<List<Card>>();
        List<int> cardIndexList = new List<int>();
        List<GameObject> cardObjList = new List<GameObject>();

        const int cardCount = 104;
        const int firstDealCardCount = 54;
        public const int nextDealCardCount = 10;
        const int maxPoint = 8;

        private int currentIndexCard = 0;
        private int currentPoint = 0;
        private int dealCount = 5;
        public int DealCount => dealCount;

        private void Awake() {
            if (instance == null) {
                instance = this;
            }

            Initial();
        }

        private void Initial() {
            for (int i = 0; i < cardTable.CardTransList.Count; i++) {
                cardStackList.Add(new List<Card>());
            }

            InitializeCardObjs();
            StartCoroutine(DealCards(firstDealCardCount));
        }

        private void InitializeCardObjs() {
            cardIndexList.Clear();
            switch (LevelManager.Instance.levelType) {
                case LevelType.Easy:
                    AddCardIndex(cardIndexList, new List<int> { 3, 3, 3, 3, 3, 3, 3, 3});
                    cardTable.Init(new List<int> { 3 });
                    break;
                case LevelType.Medium:
                    AddCardIndex(cardIndexList, new List<int> { 0, 0, 0, 0, 3, 3, 3, 3 });
                    cardTable.Init(new List<int> { 0, 3 });
                    break;
                case LevelType.Hard:
                    AddCardIndex(cardIndexList, new List<int> { 0, 1, 2, 3, 0, 1, 2, 3 });
                    cardTable.Init(new List<int> { 0, 1, 2, 3 });
                    break;
            }

            cardIndexList = ShuffleTool.ArrangeList(cardIndexList);

            cardObjList.Clear();
            foreach (int index in cardIndexList) {
                GameObject cardObj = Instantiate(cardPrefab, cardTable.DealTrans);
                cardObj.SetActive(false);

                Card card = cardObj.GetComponent<Card>();
                card.SetIndex(index);
                card.SetSprite(cardSprites[index]);

                cardObjList.Add(cardObj);
            }
        }

        private void AddCardIndex(List<int> target, List<int> suits) {
            foreach (int suit in suits) {
                target.AddRange(GetCardsBySuit(suit));
            }
        }

        private List<int> GetCardsBySuit(int suit) {
            if (suit < 0 || suit > 3) {
                return new List<int>();
            }

            List<int> cards = new List<int>();

            for (int value = 0; value <= 12; value++) {
                int cardNumber = suit * 13 + value;
                cards.Add(cardNumber);
            }

            return cards;
        }

        public IEnumerator DealCards(int numberOfCards, bool isFaceUp = false) {
            if (dealCount <= 0) yield break;

            GameManager.Instance.SetGameState(GameState.Pause);

            if (currentIndexCard != 0) dealCount--;

            int startIndex = currentIndexCard;

            int columnIndex = 0;
            int maxColumnIndex = cardTable.CardTransList.Count;

            Vector3 start = cardTable.DealTrans.position;
            for (int i = startIndex; i < startIndex + numberOfCards; i++) {
                Transform parent = cardTable.CardTransList[columnIndex];
                int rowIndex = cardStackList[columnIndex].Count;
                if (rowIndex > 0) {
                    parent = cardStackList[columnIndex][rowIndex - 1].gameObject.transform;
                }

                float spacing = rowIndex > 0 ? (isFaceUp ? spacingOfUpCard : spacingOfDownCard) : 0;
                Vector3 end = parent.position + new Vector3(0, rowIndex > 0 ? -spacing : 0, -0.1f);
            
                GameObject cardObj = cardObjList[i];
                cardObj.SetActive(true);
                StartCoroutine(MoveFromAToB(cardObj, parent, start, end, moveTime));

                Card card = cardObj.GetComponent<Card>();
                card.SetColumnIndex(columnIndex);
                card.SetRowIndex(rowIndex);
                card.SetSortingOrder(100 + rowIndex);
                StackToCardList(card, columnIndex);

                if (isFaceUp) {
                    FaceUpLastCard(columnIndex);
                }

                columnIndex++;
                if (columnIndex == maxColumnIndex) {
                    columnIndex = 0;
                }

                currentIndexCard++;

                yield return new WaitForSeconds(dealSpaceTime);

                card.SetSortingOrder(rowIndex);
            }

            if (!isFaceUp) {
                for (int i = 0; i < cardStackList.Count; i++) {
                    FaceUpLastCard(i);
                    yield return new WaitForSeconds(dealSpaceTime / 2);
                }
            }

            timeCounter.StartTimer();

            if (dealCount <= 0) {
                Destroy(cardTable.DealTrans.gameObject);
            }

            GameManager.Instance.SetGameState(GameState.Play);
        }

        IEnumerator MoveFromAToB(GameObject obj, Transform parent, Vector3 start, Vector3 end, float time, bool destroyOnEnd = false) {
            obj.transform.SetParent(parent);

            float elapsedTime = 0f;
            while (elapsedTime < time) {
                float t = elapsedTime / time;
                obj.transform.position = Vector3.Lerp(start, end, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            obj.transform.position = end;

            if (destroyOnEnd) {
                Destroy(obj);
            }
            else {
                SetSpacingOfStack(obj.GetComponent<Card>().columnIndex);
            }
        }

        private void StackToCardList(Card card, int columnIndex) {
            cardStackList[columnIndex].Add(card);
        }

        private void FaceUpLastCard(int columnIndex) {
            Animator animator = cardStackList[columnIndex][cardStackList[columnIndex].Count - 1].gameObject.GetComponent<Animator>();
            if (animator != null) {
                if (!animator.enabled) {
                    animator.enabled = true;
                }
            }
        }

        public bool MoveableCard(int columnIndex, int rowIndex) {
            if (rowIndex == cardStackList[columnIndex].Count - 1)
                return true;
            bool result = true;
            for (int i = rowIndex; i < cardStackList[columnIndex].Count - 1; i++) {
                if (cardStackList[columnIndex][i + 1].cardIndex != cardStackList[columnIndex][i].cardIndex - 1) {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public IEnumerator SetSortingOrder(Card card, int offset, float delayTime) {
            yield return new WaitForSeconds(delayTime);

            int rowIndex = card.rowIndex;
            int columnIndex = card.columnIndex;
            for (int i = rowIndex; i < cardStackList[columnIndex].Count; i++) {
                cardStackList[columnIndex][i].SetSortingOrder(offset + rowIndex);
            }
        }

        public void ReleaseCard(Card currentCard, int targetColumn = -1) {
            Vector3 start = currentCard.transform.position, end = Vector3.zero;
            Transform targetParent = currentCard.transform.parent;
            int targetRowIndex = 0;

            if (targetColumn >= 0) {
                bool move = false;

                if (cardStackList[targetColumn].Count == 0) {
                    move = true;
                    end = cardTable.CardTransList[targetColumn].position + new Vector3(0, 0, -0.1f);
                    targetParent = cardTable.CardTransList[targetColumn];
                    targetRowIndex = -1;
                }
                else {
                    Card lastCard = cardStackList[targetColumn][cardStackList[targetColumn].Count - 1];
                    int lastCardIndex = lastCard.cardIndex;
                    if (currentCard.cardIndex % 13 == (lastCardIndex % 13) - 1) {
                        move = true;
                        end = lastCard.transform.position + new Vector3(0, -spacingOfUpCard, -0.1f);
                        targetParent = lastCard.transform;
                        targetRowIndex = lastCard.rowIndex;
                    }
                }

                if (move) {
                    List<Card> moveCards = new List<Card>();
                    for (int i = currentCard.rowIndex; i < cardStackList[currentCard.columnIndex].Count; i++) {
                        moveCards.Add(cardStackList[currentCard.columnIndex][i]);
                    }
                    cardStackList[currentCard.columnIndex].RemoveRange(currentCard.rowIndex, moveCards.Count);

                    if (currentCard.rowIndex != 0) {
                        if (!cardStackList[currentCard.columnIndex][cardStackList[currentCard.columnIndex].Count - 1].isFaceUp) {
                            FaceUpLastCard(currentCard.columnIndex);
                        }
                    }

                    SetSpacingOfStack(currentCard.columnIndex);

                    for (int i = 0; i < moveCards.Count; i++) {
                        targetRowIndex += 1;
                        cardStackList[targetColumn].Add(moveCards[i]);
                        moveCards[i].SetColumnIndex(targetColumn);
                        moveCards[i].SetRowIndex(targetRowIndex);
                        moveCards[i].SetSortingOrder(targetRowIndex);
                    }

                    StartCoroutine(MoveFromAToB(currentCard.gameObject, targetParent, start, end, moveTime));

                    scoreManager.DescreaseScore();

                    int winRowIndex = CheckWin(targetColumn);
                    if (winRowIndex != -1) {
                        StartCoroutine(AddPointEffect(targetColumn, winRowIndex));
                    }

                    StartCoroutine(SetSortingOrder(currentCard, 0, moveTime));

                    return;
                }
            }

            float spacing = 0;
            if (currentCard.rowIndex >= 1) {
                spacing = cardStackList[currentCard.columnIndex][currentCard.rowIndex - 1].isFaceUp ? spacingOfUpCard : spacingOfDownCard;
            }

            end = targetParent.position + new Vector3(0, -spacing, -0.1f);
            SetSpacingOfStack(currentCard.columnIndex);
            StartCoroutine(SetSortingOrder(currentCard, 0, dealSpaceTime));
            StartCoroutine(MoveFromAToB(currentCard.gameObject, targetParent, start, end, moveTime));
        }

        private void SetSpacingOfStack(int columnIndex) {
            float heightStack = 0;
            List<Card> cardStack = cardStackList[columnIndex];
            int faceDownCardCnt = 0, faceUpCardCnt = 0;
            float factor = spacingOfUpCard / spacingOfDownCard;

            for (int i = 0; i < cardStack.Count - 1; i++) {
                if (cardStack[i].isFaceUp) {
                    heightStack += spacingOfUpCard;
                    faceUpCardCnt++;
                }
                else {
                    heightStack += spacingOfDownCard;
                    faceDownCardCnt++;
                }
            }

            float newSpacingOfUpCard = spacingOfUpCard, newSpacingOfDownCard = spacingOfDownCard;
            if (heightStack > minLimitToReSpacing) {
                newSpacingOfUpCard = (minLimitToReSpacing * factor) / (factor * faceUpCardCnt + faceDownCardCnt);
                newSpacingOfDownCard = minLimitToReSpacing / (factor * faceUpCardCnt + faceDownCardCnt);
            }

            for (int i = 1; i < cardStack.Count; i++) {
                if (cardStack[i - 1].isFaceUp) {
                    cardStack[i].transform.localPosition = new Vector3(0, -newSpacingOfUpCard, -0.1f);
                }
                else {
                    cardStack[i].transform.localPosition = new Vector3(0, -newSpacingOfDownCard, -0.1f);
                }
            }
        }

        private int CheckWin(int columnIndex) {
            int winRowIndex = -1;
            bool isWin = false;

            for (int i = 0; i < cardStackList[columnIndex].Count - 1; i++) {
                if (!cardStackList[columnIndex][i].isFaceUp) continue;
                if (winRowIndex == -1) {
                    if (cardStackList[columnIndex][i].cardIndex % 13 == 12)
                        winRowIndex = i;
                }
                if (winRowIndex != -1) {
                    if (cardStackList[columnIndex][i].cardIndex == cardStackList[columnIndex][i + 1].cardIndex + 1) {
                        if (cardStackList[columnIndex][i + 1].cardIndex % 13 == 0) {
                            isWin = true;
                            break;
                        }
                    }
                    else winRowIndex = -1;
                }
            }

            if (!isWin) {
                winRowIndex = -1;
            }
            return winRowIndex;
        }

        IEnumerator AddPointEffect(int columnIndex, int rowIndex) {
            GameManager.Instance.SetGameState(GameState.Pause);
            scoreManager.IncreaseScore();

            int suit = cardStackList[columnIndex][rowIndex].cardIndex / 13;
            Transform targetTran = cardTable.CardSuitTrans[suit];
            Vector3 start, end;

            for (int i = cardStackList[columnIndex].Count - 1; i >= rowIndex; i--) {
                cardStackList[columnIndex][i].SetSortingOrder(100 + cardStackList[columnIndex][i].cardIndex);

                GameObject cardObj = cardStackList[columnIndex][i].gameObject;
                start = cardObj.transform.position;
                end = targetTran.position;
                StartCoroutine(MoveFromAToB(cardObj, targetTran, start, end, moveTime, true));
                yield return new WaitForSeconds(dealSpaceTime);
            }

            cardStackList[columnIndex].RemoveRange(rowIndex, 13);
            SetSpacingOfStack(columnIndex);
            FaceUpLastCard(columnIndex);

            currentPoint++;
            if (currentPoint == maxPoint) {
                GameManager.Instance.FinishGame(true);
            }

            GameManager.Instance.SetGameState(GameState.Play);
        }
    }
}
