using System.Collections;
using System.Collections.Generic;
using Swift_Solitaire.Scripts.UI;
using UnityEngine;

namespace Swift_Solitaire.Scripts.Game
{
    public interface ICommand {
        void Execute();
        void Undo();
    }

    public class MoveCardCommand : ICommand {
        private CardManager _cardManager;
        private GameObject _cardObj;
        private Vector2Int _fromPos;
        private Vector2Int _toPos;
        private int _cardIndex;

        public MoveCardCommand(CardManager cardManager, GameObject cardObj, Vector2Int fromPos, Vector2Int toPos, int cardIndex) {
            _cardManager = cardManager;
            _cardObj = cardObj;
            _fromPos = fromPos;
            _toPos = toPos;
            _cardIndex = cardIndex;
        }

        public void Execute() {
            _cardManager.MoveCardToPosition(_cardObj, _fromPos, _toPos, _cardIndex);
        }

        public void Undo() {
            _cardManager.MoveCardToPosition(_cardObj, _toPos, _fromPos, _cardIndex);
        }
    }

    public class CardManager : MonoBehaviour {
        [SerializeField] List<Sprite> cardSprites;
        [SerializeField] Transform startCardTrans;
        [SerializeField] GameObject cardPrefab;
        [SerializeField] float moveTime = 1.5f;
        [SerializeField] float dealSpaceTime = 0.25f;
        [SerializeField] GridManager gridManager;
        [SerializeField] TimeCounter timeCounter;
        [SerializeField] GameObject suggestionPrefab;
        [SerializeField] GameObject selectedPrefab;

        int[,] indexOfCard;
        const int cardCount = 52;
        const int suggestionCount = 3;
        const int selectedCount = 2;

        Dictionary<int, GameObject> cardObjs = new Dictionary<int, GameObject>();
        Queue<GameObject> suggestionObjs = new Queue<GameObject>();
        Queue<GameObject> activeSuggestion = new Queue<GameObject>();
        bool isSuggestion = false;
        bool isShowSuggestion = false;
        List<GameObject> selectedObjs = new List<GameObject>();
        GameObject selectedCardObj;
        Dictionary<Vector2Int, Vector2Int> suggestions = new Dictionary<Vector2Int, Vector2Int>();

        private Stack<ICommand> commandHistory = new Stack<ICommand>();
        private int moveCount = 0;

        private void Awake() {
            indexOfCard = new int[gridManager.rows, gridManager.columns];
            InitializeCardObjs();
            InitializeBorderObjs();
        }

        private void Start() {
            InitialCards();
        }

        private void InitializeCardObjs() {
            float scaleCard = gridManager.GetTargetScale(cardSprites[0]);

            for (int i = 0; i < cardCount; i++) {
                GameObject cardObj = Instantiate(cardPrefab, startCardTrans.position, Quaternion.identity, transform);
                cardObj.transform.localScale = new Vector3(scaleCard, scaleCard, 1f);
                SpriteRenderer spriteRenderer = cardObj.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = cardSprites[i];
                cardObj.SetActive(false);

                Card card = cardObj.GetComponent<Card>();
                card.SetIndex(i);
                card.SetCardManager(this);

                if (!cardObjs.ContainsKey(i)) {
                    cardObjs.Add(i, cardObj);
                }
            }
        }

        private void InitializeBorderObjs() {
            Sprite sprite = suggestionPrefab.GetComponent<SpriteRenderer>().sprite;
            float scaleBorder = gridManager.GetTargetScale(sprite);

            for (int i = 0; i < suggestionCount; i++) {
                GameObject suggestionObj = Instantiate(suggestionPrefab, startCardTrans.position, Quaternion.identity, transform);
                suggestionObj.transform.localScale = new Vector3(scaleBorder, scaleBorder, 1f);
                suggestionObj.SetActive(false);
                suggestionObjs.Enqueue(suggestionObj);
            }

            for (int i = 0; i < selectedCount; i++) {
                GameObject selectedObj = Instantiate(selectedPrefab, startCardTrans.position, Quaternion.identity, transform);
                selectedObj.transform.localScale = new Vector3(scaleBorder, scaleBorder, 1f);
                selectedObj.SetActive(false);
                selectedObjs.Add(selectedObj);

                Card card = selectedObj.GetComponent<Card>();
                card.SetCardManager(this);
            }
        }

        private void InitialCards() {
            GameManager.Instance.SetGameState(GameState.Pause);

            int[] cardArray = new int[cardCount];
            for (int i = 0; i < cardCount; i++) {
                cardArray[i] = i;
                indexOfCard[i / gridManager.columns, i % gridManager.columns] = -1;
            }

            int[] sortedCard = ShuffleTool.ArrangeArray(cardArray, gridManager.rows, gridManager.columns, false);
            StartCoroutine(DealCard(sortedCard));

            isSuggestion = false;
            isShowSuggestion = false;
            commandHistory.Clear();
            moveCount = 0;
        }

        IEnumerator DealCard(int[] sortedCard) {
            Dictionary<Vector2Int, GameObject> aceCards = new Dictionary<Vector2Int, GameObject>();

            for (int i = 0; i < gridManager.rows; i++) {
                for (int j = 0; j < gridManager.columns; j++) {
                    int index = i * gridManager.columns + j;
                    GameObject cardObj;
                    int cardIndex = sortedCard[index];
                    if (cardObjs.TryGetValue(cardIndex, out cardObj)) {
                        cardObj.SetActive(true);
                        Vector2Int coordinate = new Vector2Int(index % gridManager.columns, index / gridManager.columns);
                        StartCoroutine(MoveFromAToB(cardObj, startCardTrans.position, gridManager.GetGridPosition(coordinate), moveTime));
                        Card card = cardObj.GetComponent<Card>();
                        card.SetCoordinate(coordinate);

                        indexOfCard[i, j] = cardIndex;

                        if (cardIndex % gridManager.columns == gridManager.columns - 1) {
                            aceCards.Add(new Vector2Int(i, j), cardObj);
                        }

                        yield return new WaitForSeconds(dealSpaceTime);
                    }
                }
            }

            yield return new WaitForSeconds(moveTime);

            foreach (KeyValuePair<Vector2Int, GameObject> cardObj in aceCards) {
                StartCoroutine(MoveFromAToB(cardObj.Value, cardObj.Value.transform.position, startCardTrans.position, moveTime));
            }

            yield return new WaitForSeconds(moveTime);

            foreach (KeyValuePair<Vector2Int, GameObject> cardObj in aceCards) {
                cardObj.Value.SetActive(false);
                indexOfCard[cardObj.Key.x, cardObj.Key.y] = -1;
            }

            SuggestionHandle();
            timeCounter.StartTimer();
            GameManager.Instance.SetGameState(GameState.Play);
        }

        IEnumerator MoveFromAToB(GameObject obj, Vector2 start, Vector2 end, float time) {
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            int sortingOrder = spriteRenderer.sortingOrder;
            spriteRenderer.sortingOrder = sortingOrder + 1;

            float elapsedTime = 0f;
            while (elapsedTime < time) {
                float t = elapsedTime / time;
                obj.transform.position = Vector3.Lerp(start, end, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            obj.transform.position = end;
            spriteRenderer.sortingOrder = sortingOrder;
        }

        private void SuggestionHandle() {
            if (isSuggestion) return;

            suggestions.Clear();

            for (int i = 0; i < gridManager.rows; i++) {
                for (int j = 0; j < gridManager.columns; j++) {
                    if (indexOfCard[i, j] == -1) {
                        if (j == 0) {
                            int index;
                            for (int k = 0; k < gridManager.rows; k++) {
                                index = k * gridManager.columns;
                                GameObject cardObj;
                                if (cardObjs.TryGetValue(index, out cardObj)) {
                                    Card card = cardObj.GetComponent<Card>();
                                    suggestions.Add(card.coordinate, new Vector2Int(j, i));
                                    break;
                                }
                            }
                        }
                        else {
                            int targetIndex = indexOfCard[i, j - 1] + 1;
                            if (targetIndex % gridManager.columns == 0 || targetIndex % gridManager.columns == gridManager.columns - 1) {
                                continue;
                            }

                            GameObject cardObj;
                            if (cardObjs.TryGetValue(targetIndex, out cardObj)) {
                                Card card = cardObj.GetComponent<Card>();
                                suggestions.Add(card.coordinate, new Vector2Int(j, i));
                            }
                        }
                    }
                }
            }

            FindAllMoveableCard();
            isSuggestion = true;
        }

        private void FindAllMoveableCard() {
            for (int i = 0; i < gridManager.rows; i++) {
                for (int j = 0; j < gridManager.columns; j++) {
                    if (indexOfCard[i, j] == -1) {
                        if (j == 0) {
                            int index;
                            for (int k = 0; k < gridManager.rows; k++) {
                                index = k * gridManager.columns;
                                GameObject cardObj;
                                if (cardObjs.TryGetValue(index, out cardObj)) {
                                    Card card = cardObj.GetComponent<Card>();
                                    if (!suggestions.ContainsKey(card.coordinate)) {
                                        suggestions.Add(card.coordinate, new Vector2Int(j, i));
                                    }
                                }
                            }
                        }
                        else {
                            int targetIndex = indexOfCard[i, j - 1] + 1;
                            if (targetIndex % gridManager.columns == 0 || targetIndex % gridManager.columns == gridManager.columns - 1) {
                                continue;
                            }

                            GameObject cardObj;
                            if (cardObjs.TryGetValue(targetIndex, out cardObj)) {
                                Card card = cardObj.GetComponent<Card>();
                                if (!suggestions.ContainsKey(card.coordinate)) {
                                    suggestions.Add(card.coordinate, new Vector2Int(j, i));
                                }
                            }
                        }
                    }
                }
            }
        }

        public void DisplaySuggestion(bool isShow) {
            if (GameManager.Instance.IsPaused()) return;

            if (isShow) {
                if (isShowSuggestion) return;

                foreach (KeyValuePair<Vector2Int, Vector2Int> pos in suggestions) {
                    GameObject suggestionObj;
                    if (suggestionObjs.TryDequeue(out suggestionObj)) {
                        suggestionObj.SetActive(true);
                        suggestionObj.transform.position = gridManager.GetGridPosition(pos.Key);
                        activeSuggestion.Enqueue(suggestionObj);
                    }
                }

                isShowSuggestion = true;
                HideSelectedCard();
            }
            else {
                while (activeSuggestion.Count > 0) {
                    GameObject suggestionObj = activeSuggestion.Dequeue();
                    suggestionObjs.Enqueue(suggestionObj);
                    suggestionObj.SetActive(false);
                }
                isShowSuggestion = false;
            }
        }

        public void SelectCard(GameObject cardObj, Vector2Int pos) {
            Vector2Int targetPos;
            if (suggestions.TryGetValue(pos, out targetPos)) {
                if (selectedCardObj == cardObj) {
                    MoveCard(targetPos);
                    return;
                }

                selectedCardObj = cardObj;
                ShowSelectedCards(pos, targetPos);
            }
            else {
                MoveCard(pos);
            }
        }

        private void MoveCard(Vector2Int pos) {
            if (GameManager.Instance.IsPaused()) return;

            foreach (KeyValuePair<Vector2Int, Vector2Int> keyValuePair in suggestions) {
                if (keyValuePair.Value == pos) {
                    Card card = selectedCardObj.GetComponent<Card>();
                    if (card.coordinate == keyValuePair.Key) {
                        ICommand command = new MoveCardCommand(this, selectedCardObj, keyValuePair.Key, keyValuePair.Value, card.cardIndex);
                        command.Execute();
                        commandHistory.Push(command);
                        moveCount++;

                        HideSelectedCard();
                        isSuggestion = false;
                        SuggestionHandle();

                        if (CheckWin()) {
                            GameManager.Instance.FinishGame();
                        }
                        break;
                    }
                }
            }
        }

        public void MoveCardToPosition(GameObject cardObj, Vector2Int fromPos, Vector2Int toPos, int cardIndex) {
            StartCoroutine(MoveCardCoroutine(cardObj, fromPos, toPos, cardIndex));
        }

        private IEnumerator MoveCardCoroutine(GameObject cardObj, Vector2Int fromPos, Vector2Int toPos, int cardIndex) {
            yield return StartCoroutine(MoveFromAToB(cardObj, gridManager.GetGridPosition(fromPos), gridManager.GetGridPosition(toPos), moveTime));

            Card card = cardObj.GetComponent<Card>();
            card.SetCoordinate(toPos);

            indexOfCard[toPos.y, toPos.x] = cardIndex;
            indexOfCard[fromPos.y, fromPos.x] = -1;

            isSuggestion = false;
            SuggestionHandle();
        }

        private bool CheckWin() {
            for (int i = 0; i < gridManager.rows; i++) {
                if (indexOfCard[i, 12] != -1)
                    return false;

                int expectedValue = indexOfCard[i, 0];
                if (expectedValue % gridManager.columns != 0) return false;

                for (int j = 1; j < gridManager.columns - 1; j++) {
                    if (indexOfCard[i, j] != expectedValue + 1)
                        return false;
                    expectedValue = indexOfCard[i, j];
                }
            }
            return true;
        }

        public int GetMoveCount() => moveCount;

        private void ShowSelectedCards(Vector2Int pos, Vector2Int targetPos) {
            DisplaySuggestion(false);
            ShowSelectedCard(selectedObjs[0], pos, false);
            ShowSelectedCard(selectedObjs[1], targetPos, true);
        }

        private void ShowSelectedCard(GameObject selectedCard, Vector2Int pos, bool hasCollider) {
            selectedCard.SetActive(true);
            selectedCard.transform.position = gridManager.GetGridPosition(pos);
            Card card = selectedCard.GetComponent<Card>();
            card.SetCoordinate(pos);
            Collider2D col = selectedCard.GetComponent<Collider2D>();
            col.enabled = hasCollider;
        }

        private void HideSelectedCard() {
            selectedObjs[0].SetActive(false);
            selectedObjs[1].SetActive(false);
            selectedCardObj = null;
        }

        public void Reset() {
            StopAllCoroutines();
            StartCoroutine(ResetHandle());
        }

        IEnumerator ResetHandle() {
            timeCounter.Reset();
            DisplaySuggestion(false);
            HideSelectedCard();
            suggestions.Clear();
            commandHistory.Clear();

            for (int i = 0; i < gridManager.rows; i++) {
                for (int j = 0; j < gridManager.columns; j++) {
                    if (indexOfCard[i, j] != -1) {
                        GameObject cardObj;
                        if (cardObjs.TryGetValue(indexOfCard[i, j], out cardObj)) {
                            StartCoroutine(MoveFromAToB(cardObj, cardObj.transform.position, startCardTrans.position, moveTime));
                        }
                    }
                }
            }

            yield return new WaitForSeconds(moveTime);

            InitialCards();
        }

        public void Undo() {
            if (GameManager.Instance.IsPaused() || commandHistory.Count == 0)
                return;

            ICommand command = commandHistory.Pop();
            command.Undo();
        }
    }
}