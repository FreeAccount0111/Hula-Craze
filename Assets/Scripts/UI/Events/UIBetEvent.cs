using System;
using Data;
using UnityEngine;

namespace UI.Events
{
    public class UIBetEvent
    {
        public static event Action OnIncreaseBet;
        public static event Action OnReductionBet;
        public static event Action OnIncreaseLine;
        public static event Action OnReductionLine;
        public static event Action OnMaxBet;
        public static event Action OnClickSpin;
        public static event Action<int> OnUpdateWin;
        public static event Action<UserData> OnUpdateData;
        public static event Action<bool> OnSpinSuccess;

        public static void RaiseIncreaseBet() => OnIncreaseBet?.Invoke();
        public static void RaiseReductionBet() => OnReductionBet?.Invoke();
        public static void RaiseIncreaseLine() => OnIncreaseLine?.Invoke();
        public static void RaiseReductionLine() => OnReductionLine?.Invoke();
        public static void RaiseMaxBet() => OnMaxBet?.Invoke();
        public static void RaiseClickSpin() => OnClickSpin?.Invoke();
        public static void RaiseSpinSuccess(bool en) => OnSpinSuccess?.Invoke(en);
        public static void RaiseUpdateWin(int coin) => OnUpdateWin?.Invoke(coin);
        public static void RaiseUpdateData(UserData data) => OnUpdateData?.Invoke(data);
    }
}
