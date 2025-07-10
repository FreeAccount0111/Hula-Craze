using System.Collections.Generic;
using DG.Tweening;
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

        public void ShowBacGround()
        {
            background.color = new Color(0, 0, 0, 0);
            background.gameObject.SetActive(true);
            background.DOFade(0.8f, 0.5f);
        }
        
        public void ShowBigWin(int amount)
        {
            bigWinSlot.SetWinText(amount);
            bigWinSlot.Show();
        }

        public void ShowMegaWin(int amount)
        {
            megaWinSlot.SetWinText(amount);
            megaWinSlot.Show();
        }

        public void ShowSuperWin(int amount)
        {
            superWinSlot.SetWinText(amount);
            superWinSlot.Show();
        }

        public void ShowSpinFree(int spin)
        {
            spinWinSlot.SetWinText(spin);
            spinWinSlot.Show();
        }
    }
}
