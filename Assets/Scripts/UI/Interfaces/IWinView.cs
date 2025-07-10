using UnityEngine;

namespace UI.Interfaces
{
    public interface IWinView
    {
        public void ShowBacGround();
        public void HideBackGround();
        public void ShowBigWin(int amount);
        public void HideBigWin();
        public void ShowMegaWin(int amount);
        public void HideMegaWin();
        public void ShowSuperWin(int amount);
        public void HideSuperWin();
        public void ShowSpinFree(int spin);
        public void HideSpinFree();
        public void UpdateWinText(int win);
    }
}
