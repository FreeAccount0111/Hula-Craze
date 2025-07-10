using System;
using TMPro;
using UI.Events;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Views
{
    public class BetView : MonoBehaviour, IBetView
    {
        [SerializeField] private TMP_Text textCredit;
        [SerializeField] private TMP_Text textBet;
        [SerializeField] private TMP_Text textLine;
        [SerializeField] private TMP_Text textTotal;
        [SerializeField] private TMP_Text textSpin;
        [SerializeField] private TMP_Text textWin;
        [SerializeField] private TMP_Text textNotification;
        [SerializeField] private GameObject htpObj;
        
        [SerializeField] private Button btnIncreaseBet;
        [SerializeField] private Button btnReductionBet;
        [SerializeField] private Button btnIncreaseLine;
        [SerializeField] private Button btnReductionLine;
        [SerializeField] private Button btnSpin;
        [SerializeField] private Button btnSpinFree;
        [SerializeField] private Button btnHtp;
        [SerializeField] private Button btnExitHtp;
        [SerializeField] private Button btnMaxBet;

        private void Awake()
        {
            btnIncreaseBet.onClick.AddListener(UIBetEvent.RaiseIncreaseBet);
            btnReductionBet.onClick.AddListener(UIBetEvent.RaiseReductionBet);
            btnIncreaseLine.onClick.AddListener(UIBetEvent.RaiseIncreaseLine);
            btnReductionLine.onClick.AddListener(UIBetEvent.RaiseReductionLine);
            btnMaxBet.onClick.AddListener(UIBetEvent.RaiseMaxBet);
            btnSpin.onClick.AddListener(UIBetEvent.RaiseClickSpin);
            btnSpinFree.onClick.AddListener(UIBetEvent.RaiseClickSpin);
            btnHtp.onClick.AddListener(UIBetEvent.RaiseShowHtp);
            btnExitHtp.onClick.AddListener(UIBetEvent.RaiseHideHtp);
        }

        public void UpdateTotalText(int total)
        {
            textTotal.text = FormatNumber(total);
        }

        public void UpdateCreditText(int credit)
        {
            textCredit.text = FormatNumber(credit);
        }
        public void UpdateSpin(bool en)
        {
            btnIncreaseBet.gameObject.SetActive(!en);
            btnReductionBet.gameObject.SetActive(!en);
            btnIncreaseLine.gameObject.SetActive(!en);
            btnReductionLine.gameObject.SetActive(!en);
            btnSpin.gameObject.SetActive(!en);
            btnSpinFree.gameObject.SetActive(en);
        }

        public void UpdateSpinText(int spin)
        {
            textSpin.text = spin.ToString();
        }

        public void ShowHtp()
        {
            htpObj.SetActive(true);
        }

        public void HideHtp()
        {
            htpObj.SetActive(false);
        }

        public void UpdateWinText(int amount)
        {
            textWin.text = FormatNumber(amount);
        }

        public void UpdateBetText(int amount)
        {
            textBet.text = FormatNumber(amount);
        }

        public void UpdateLineText(int line)
        {
            textLine.text = FormatNumber(line);
        }

        public void UpdateNotification(string s)
        {
            textNotification.text = s;
        }

        private string FormatNumber(int amount)
        {
            if (amount == 0)
                return "0";
            
            string s = "";
            while (amount > 0)
            {
                if (amount >= 1000)
                {
                    s = $",{amount % 1000:000}" + s;
                }
                else
                {
                    s = amount.ToString() + s;
                }

                amount = amount / 1000;
            }
            return s;
        }
    }
}
