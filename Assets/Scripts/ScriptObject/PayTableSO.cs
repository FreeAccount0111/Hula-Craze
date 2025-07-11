using System;
using System.Collections.Generic;
using Gameplay.Controller;
using UnityEngine;

namespace ScriptObject
{
    [Serializable]
    public struct SymbolPayoutEntry
    {
        public SymbolType symbol;
        public int[] payouts;
    }
    
    [CreateAssetMenu(menuName = "DataSO/PayTableSO")]
    public class PayTableSo : ScriptableObject
    {
        public List<SymbolPayoutEntry> payoutEntries;
    }
}
