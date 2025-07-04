using System;
using TMPro;
using UI.Events;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class BetView : MonoBehaviour, IBetView
    {
        [SerializeField] private TMP_Text textBet;
        [SerializeField] private TMP_Text textWin;
        [SerializeField] private TMP_Text textNotification;
        
        [SerializeField] private Button btnIncrease, btnReduction;
        [SerializeField] private Button btnSpin;
        [SerializeField] private Button btnHtp;
        [SerializeField] private Button btnMaxBet;

        private void Awake()
        {
            btnIncrease.onClick.AddListener(UIBetEvent.RaiseIncreaseBet);
            btnReduction.onClick.AddListener(UIBetEvent.RaiseReductionBet);
            btnMaxBet.onClick.AddListener(UIBetEvent.RaiseMaxBet);
            btnSpin.onClick.AddListener(UIBetEvent.RaiseClickSpin);
        }

        public void UpdateWinText(int amount)
        {
            textWin.text = $"{amount}$";
        }

        public void UpdateBetText(int amount)
        {
            textBet.text = $"{amount}$";
        }

        public void UpdateNotification(string s)
        {
            textNotification.text = s;
        }
    }
}
