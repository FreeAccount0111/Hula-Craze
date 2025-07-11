using TMPro;
using UI.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class LobbyView : MonoBehaviour,ILobbyView
    {
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Image fullImg;
        
        public void UpdateCoinText(int coin)
        {
            coinText.text = FormatNumber(coin);
        }

        public void UpdateLevelText(int level)
        {
            levelText.text = level.ToString();
        }

        public void UpdateLevelExp(float amount)
        {
            fullImg.fillAmount = amount;
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
