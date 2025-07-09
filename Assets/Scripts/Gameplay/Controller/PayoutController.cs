using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Gameplay.Events;
using Gameplay.Manager;
using ScriptObject;
using UI.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Controller
{
    public class PayoutController : MonoBehaviour
    {
        private UserModel _userModel;
        private readonly CellController[,] _board = new CellController[4,5];
        [SerializeField] private PayTableSo payTable;
        private readonly List<List<(int x, int y)>> _paylines = new List<List<(int, int)>>()
        {
            new() { (0, 0), (0, 1), (0, 2), (0, 3), (0, 4) },
            new() { (1, 0), (1, 1), (1, 2), (1, 3), (1, 4) },
            new() { (2, 0), (2, 1), (2, 2), (2, 3), (2, 4) },
            new() { (3, 0), (3, 1), (3, 2), (3, 3), (3, 4) },

            new() { (0, 0), (1, 1), (0, 2), (1, 3), (0, 4) },
            new() { (1, 0), (0, 1), (1, 2), (0, 3), (1, 4) },
            new() { (1, 0), (2, 1), (1, 2), (2, 3), (1, 4) },
            new() { (2, 0), (1, 1), (2, 2), (1, 3), (2, 4) },
            new() { (2, 0), (3, 1), (2, 2), (3, 3), (2, 4) },
            new() { (3, 0), (2, 1), (3, 2), (2, 3), (3, 4) },
            
            new() { (0, 0), (1, 1), (2, 2), (1, 3), (0, 4) },
            new() { (1, 0), (2, 1), (3, 2), (2, 3), (1, 4) },
            new() { (3, 0), (2, 1), (1, 2), (2, 3), (3, 4) },
            new() { (2, 0), (1, 1), (0, 2), (1, 3), (2, 4) },
            
            new() { (0, 0), (0, 1), (1, 2), (0, 3), (0, 4) },
            new() { (1, 0), (1, 1), (2, 2), (1, 3), (1, 4) },
            new() { (2, 0), (2, 1), (3, 2), (2, 3), (2, 4) },
            new() { (3, 0), (3, 1), (2, 2), (3, 3), (3, 4) },
            new() { (2, 0), (2, 1), (1, 2), (2, 3), (2, 4) },
            new() { (1, 0), (1, 1), (0, 2), (1, 3), (1, 4) },
            
            new() { (0, 0), (1, 1), (2, 2), (2, 3), (2, 4) },
            new() { (1, 0), (2, 1), (3, 2), (3, 3), (3, 4) },
            new() { (2, 0), (2, 1), (2, 2), (1, 3), (0, 4) },
            new() { (3, 0), (3, 1), (3, 2), (2, 3), (1, 4) },
        };

        private readonly List<int> _indexLineWin = new List<int>();
        private readonly List<List<(int x, int y)>> _lsLineWin = new List<List<(int x, int y)>>();
        
        [SerializeField] private int[] indexSymbol = new int[13];
        [SerializeField] private int coinPayout;
        [SerializeField] private int spinFree;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => UserManager.Instance != null);
            _userModel = UserManager.Instance.userModel;
        }

        public void CheckResult(CellController[,] board)
        {
            for (int i = 0; i < 5; i++)
            for (int j = 0; j < 4; j++)
                _board[j, i] = board[j, i];
            
            coinPayout = 0;
            spinFree = 0;
            _indexLineWin.Clear();
            _lsLineWin.Clear();

            for (int i = 0; i < _userModel.UserData.currentLine; i++)
                CalculatorPayLine(i);
            
            GameEvent.RaisePayout(coinPayout,spinFree);
            if (coinPayout > 0)
                GameEvent.RaiseShowLineWin(_indexLineWin,_lsLineWin);
        }

        private void CalculatorPayLine(int index)
        {
            for (int i = 0; i < 13; i++)
                indexSymbol[i] = 0;
            
            var line = _paylines[index];
            for (int i = 0; i < line.Count; i++)
                indexSymbol[(int)_board[line[i].x, line[i].y].SymbolType] += 1;

            if (indexSymbol[12] >= 3)
            {
                spinFree += payTable.payoutEntries[12].payouts[indexSymbol[12]];
                _indexLineWin.Add(index);
                _lsLineWin.Add(line);
            }
            else
            {
                var amountPayout = 0;
                for (int i = 0; i < 11; i++)
                    amountPayout += _userModel.UserData.currentBet * (payTable.payoutEntries[i].payouts[indexSymbol[i] + indexSymbol[11]]);
                if (amountPayout > 0)
                {
                    coinPayout += amountPayout;
                    _indexLineWin.Add(index);
                    _lsLineWin.Add(line);
                }
            }
        }
    }
}
