using System;
using System.Collections.Generic;
using Gameplay.Controller;
using UnityEngine;

namespace ScriptObject
{
    [System.Serializable]
    public class SymbolPayoutEntry
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
