using System;

namespace Gameplay.Events
{
   public class GameEvent
   {
      public static event Action OnSpin;

      public static void RaiseSpin() => OnSpin?.Invoke();
   }
}
