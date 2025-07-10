using TMPro;
using UI.Interfaces;
using UnityEngine;

namespace UI.Views
{
    public class G2WinView : MonoBehaviour,IWinView
    {
        [SerializeField] private TMP_Text winText;
        public void ShowBacGround()
        {
        
        }

        public void HideBackGround()
        {
        
        }

        public void ShowBigWin(int amount)
        {
        
        }

        public void HideBigWin()
        {
       
        }

        public void ShowMegaWin(int amount)
        {
      
        }

        public void HideMegaWin()
        {
       
        }

        public void ShowSuperWin(int amount)
        {
       
        }

        public void HideSuperWin()
        {

        }

        public void ShowSpinFree(int spin)
        {

        }

        public void HideSpinFree()
        {

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
