using System.Collections.Generic;
using UnityEngine;

namespace Deck_Dominion.Scripts.Game
{
    public class CardTable : MonoBehaviour
    {
        [SerializeField] List<Transform> cardTransList = new List<Transform>();
        [SerializeField] List<Transform> targetTransList = new List<Transform>();
        [SerializeField] List<Sprite> cardSuitSpriteList = new List<Sprite>();
        [SerializeField] Transform dealTrans;
        Dictionary<int, Transform> cardSuitTrans = new Dictionary<int, Transform>();

        public List<Transform> CardTransList => cardTransList;
        public Dictionary<int, Transform> CardSuitTrans => cardSuitTrans;
        public Transform DealTrans => dealTrans;

        public void Init(List<int> cardSuits) {
            for (int i = 0; i < cardSuits.Count; i++) {
                SpriteRenderer spriteRenderer = targetTransList[i].GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = cardSuitSpriteList[cardSuits[i]];
                cardSuitTrans.Add(cardSuits[i], targetTransList[i]);
            }
        }
    }
}
