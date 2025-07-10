using UnityEngine;

namespace UI.Interfaces
{
    public interface IWinView
    {
        public void ShowBacGround();
        public void ShowBigWin(int amount);
        public void ShowMegaWin(int amount);
        public void ShowSuperWin(int amount);
        public void ShowSpinFree(int spin);
    }
}
