using System;
using System.Collections.Generic;

namespace Gameplay.Events
{
   public class GameEvent
   {
      public static event Action<int,int> OnSpin;
      public static event Action OnResetRoll;
      public static event Action<int, int> OnPayout;
      public static event Action<List<int>, List<List<(int x, int y)>>> OnShowLineWin;

      public static void RaiseSpin(int betAmount,int payline) => OnSpin?.Invoke(betAmount,payline);
      public static void RaiseResetRoll() => OnResetRoll?.Invoke();
      public static void RaisePayout(int coin, int freeSpin) => OnPayout?.Invoke(coin, freeSpin);
      public static void RaiseShowLineWin(List<int> indexLine, List<List<(int x, int y)>> lineWin) => OnShowLineWin?.Invoke(indexLine, lineWin);
   }
}
