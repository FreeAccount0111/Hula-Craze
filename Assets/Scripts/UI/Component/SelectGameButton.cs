using System;
using UI.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Component
{
    public class SelectGameButton : MonoBehaviour
    {
        [SerializeField] private Button btn;
        [SerializeField] private string nameGame;

        private void Awake()
        {
            btn.onClick.AddListener(() => UILobbyEvent.RaiseLoadGame(nameGame));
        }
    }
}
