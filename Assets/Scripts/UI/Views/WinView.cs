using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UI.Component;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class WinView : MonoBehaviour,IWinView
    {
        [SerializeField] private WinSlot bigWinSlot;
        [SerializeField] private WinSlot megaWinSlot;
        [SerializeField] private WinSlot superWinSlot;
        [SerializeField] private WinSlot spinWinSlot;
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text winText;

        public void ShowBacGround()
        {
            background.color = new Color(0, 0, 0, 0);
            background.gameObject.SetActive(true);
            background.DOFade(0.8f, 0.5f);
        }

        public void HideBackGround()
        {
            background.gameObject.SetActive(false);
        }

        public void ShowBigWin(int amount)
        {
            bigWinSlot.SetWinText($"BIG WIN! - {FormatNumber(amount)} COINS!");
            bigWinSlot.Show();
        }

        public void HideBigWin()
        {
            bigWinSlot.Hide();
        }

        public void ShowMegaWin(int amount)
        {
            megaWinSlot.SetWinText($"MEGA WIN! - {FormatNumber(amount)} COINS!");
            megaWinSlot.Show();
        }

        public void HideMegaWin()
        {
            megaWinSlot.Hide();
        }

        public void ShowSuperWin(int amount)
        {
            superWinSlot.SetWinText($"SUPER WIN! - {FormatNumber(amount)} COINS!");
            superWinSlot.Show();
        }

        public void HideSuperWin()
        {
            superWinSlot.Hide();
        }

        public void ShowSpinFree(int spin)
        {
            spinWinSlot.SetWinText($"Congratulations! You've won {spin} Free Spins!");
            spinWinSlot.Show();
        }

        public void HideSpinFree()
        {
            spinWinSlot.Hide();
        }

        public void UpdateWinText(int win)
        {
            winText.text = FormatNumber(win);
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
