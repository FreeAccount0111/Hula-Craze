namespace UI.Interfaces
{
    public interface IBetView
    {
        public void UpdateBetText(int amount);
        public void UpdateLineText(int line);
        public void UpdateTotalText(int total);
        public void UpdateCreditText(int credit);
        public void UpdateWinText(int amount);
        public void UpdateNotification(string s);
    }
}
