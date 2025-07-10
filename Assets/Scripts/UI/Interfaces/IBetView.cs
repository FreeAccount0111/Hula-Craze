namespace UI.Interfaces
{
    public interface IBetView
    {
        public void UpdateBetText(int amount);
        public void UpdateLineText(int line);
        public void UpdateTotalText(int total);
        public void UpdateCreditText(int credit);
        public void UpdateSpin(bool en);
        public void UpdateSpinText(int spin);
        public void ShowHtp();
        public void HideHtp();
        public void UpdateWinText(int amount);
        public void UpdateNotification(string s);
    }
}
