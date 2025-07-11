using System;
using System.Collections;
using Deck_Dominion.Scripts.Game;
using TMPro;
using UnityEngine;

namespace Deck_Dominion.Scripts.UI
{
    public class TimeCounter : MonoBehaviour
    {
        [SerializeField] TMP_Text timeText;
        [SerializeField] float duration = 300f;

        public Action OnTimerFinished;

        private float timeRemaining;
        private bool isStarted = false;

        private void Start() {
            UpdateTimeText(duration);
        }

        public void StartTimer() {
            if (isStarted) return;

            isStarted = true;
            timeRemaining = duration;
            StartCoroutine(Countdown());
        }

        IEnumerator Countdown() {
            while (timeRemaining > 0f) {
                UpdateTimeText(timeRemaining);
                timeRemaining -= Time.deltaTime;
                yield return null;
            }

            UpdateTimeText(0f);

            GameManager.Instance.FinishGame(false);
        }

        public void UpdateTimeText(float time) {
            TimeSpan ts = TimeSpan.FromSeconds(time);
            timeText.text = $"{ts.Minutes:D2} : {ts.Seconds:D2}";
        }

        public void Reset() {
            StopAllCoroutines();
            UpdateTimeText(duration);
        }

        public float GetTimeRemaining() => timeRemaining;
    }
}
