using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class DataBet
    {
        public int currentBet = 100;
        public int currentLine = 1;
        public int freeSpins = 0;
    }
    [Serializable]
    public class UserData
    {
        public int currentCoin = 50000;
        public List<DataBet> dataBets = new List<DataBet>()
        {
            new DataBet(),
            new DataBet(),
            new DataBet()
        };
    }
}
