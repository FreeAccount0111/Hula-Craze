using System.Collections.Generic;
using UI.Component;
using UI.Interfaces;
using UnityEngine;

namespace UI.Views
{
    public class WinView : MonoBehaviour,IWinView
    {
        [SerializeField] private List<WinSlot> slots = new List<WinSlot>();
    }
}
