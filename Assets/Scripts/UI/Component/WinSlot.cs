using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Component
{
    public class WinSlot : MonoBehaviour
    {
        [SerializeField] protected Text textWin;
        [SerializeField] private CanvasGroup canvasGroup;

        public void SetWinText(string s)
        {
            textWin.text = s;
        }

        public void Show()
        {
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            //canvasGroup.DOFade(1, 0.5f).SetEase(Ease.Linear);
            transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutCirc);
        }

        public void Hide()
        {
            transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InCirc).OnComplete(()=> gameObject.SetActive(false));
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
