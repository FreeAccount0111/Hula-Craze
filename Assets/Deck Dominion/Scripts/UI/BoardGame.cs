using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Deck_Dominion.Scripts.UI
{
    public class BoardGame : MonoBehaviour {
        public RectTransform frame;
        public Button pullUpButton;
        public Button pullDownButton;
        public float slideSpeed = 500f;

        private Vector2 startPos;
        private Vector2 endPos;
        private bool isSliding = false;
        private bool isOpen = true;

        void Start() {
            endPos = frame.anchoredPosition;
            startPos = new Vector2(endPos.x, endPos.y - frame.rect.height); 

            pullUpButton.onClick.AddListener(OnPullUp);
            pullDownButton.onClick.AddListener(OnPullDown);

            pullUpButton.gameObject.SetActive(false);
            pullDownButton.gameObject.SetActive(true);
        }

        void OnPullUp() {
            if (!isSliding) {
                isSliding = true;
                isOpen = true;
                StartCoroutine(SlideFrame());
            }
        }

        void OnPullDown() {
            if (!isSliding) {
                isSliding = true;
                isOpen = false;
                StartCoroutine(SlideFrame());
            }
        }

        IEnumerator SlideFrame() {
            while (isSliding) {
                float step = slideSpeed * Time.deltaTime;
                if (isOpen) {
                    frame.anchoredPosition = Vector2.MoveTowards(frame.anchoredPosition, endPos, step);
                    if (Vector2.Distance(frame.anchoredPosition, endPos) < 0.1f) {
                        frame.anchoredPosition = endPos;
                        isSliding = false;
                        pullUpButton.gameObject.SetActive(false);
                        pullDownButton.gameObject.SetActive(true);
                    }
                }
                else {
                    frame.anchoredPosition = Vector2.MoveTowards(frame.anchoredPosition, startPos, step);
                    if (Vector2.Distance(frame.anchoredPosition, startPos) < 0.1f) {
                        frame.anchoredPosition = startPos;
                        isSliding = false;
                        pullUpButton.gameObject.SetActive(true);
                        pullDownButton.gameObject.SetActive(false);
                    }
                }
                yield return null;
            }
        }
    }
}