using UnityEngine;
using UnityEngine.UI;

namespace Swift_Solitaire.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class CustomBtn : MonoBehaviour
    {
        protected Button btn;

        protected virtual void Awake() {
            btn = GetComponent<Button>();
        }

        protected virtual void Start() {
            btn.onClick.AddListener(ClickedHandle);
        }

        protected virtual void ClickedHandle() { }
    }
}
