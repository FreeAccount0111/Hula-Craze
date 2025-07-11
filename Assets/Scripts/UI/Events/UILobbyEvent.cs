using System;
using UnityEngine;

namespace UI.Events
{
    public class UILobbyEvent
    {
        public static event Action<string> OnLoadGame;

        public static void RaiseLoadGame(string nameScene) => OnLoadGame?.Invoke(nameScene);
    }
}
