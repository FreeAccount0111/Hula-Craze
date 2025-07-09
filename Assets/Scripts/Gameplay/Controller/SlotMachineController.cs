using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Events;
using UnityEngine;

namespace Gameplay.Controller
{
    public class SlotMachineController : MonoBehaviour
    {
        public static SlotMachineController Instance;

        [SerializeField] private PayoutController payoutController;
        [SerializeField] private LineController lineController;
        
        [SerializeField] private List<ReelController> reels = new List<ReelController>();
        private readonly CellController[,] _board = new CellController[4,5];
        [SerializeField] private bool[] checkEndSpin;
        [SerializeField] private float speedRoll;
        [SerializeField] private int countRoll;
        [SerializeField] private int indexLastCount;

        private Coroutine _coroutineRoll;
        private Coroutine _coroutineWin;

        private bool _isRolling;

        private void Awake()
        {
            SlotMachineController.Instance = this;
        }
        IEnumerator Start()
        {
            yield return new WaitUntil(() => ObjectPool.Instance != null);
            FillBoard();
            checkEndSpin = new bool[reels.Count];
        }
        private void OnEnable()
        {
            GameEvent.OnSpin += Roll;
            GameEvent.OnShowLineWin += ShowWin;
        }
        private void OnDisable()
        {
            GameEvent.OnSpin -= Roll;
            GameEvent.OnShowLineWin -= ShowWin;
        }
        private void FillBoard()
        {
            foreach (var reel in reels)
            {
                reel.FillReel();
            }
        }
        private void Roll()
        {
            if (_isRolling)
                return;

            StopShowWin();
            _isRolling = true;
            for (int i = 0; i < reels.Count; i++)
                checkEndSpin[i] = false;
            
            _coroutineRoll = StartCoroutine(RollCoroutine());
        }
        IEnumerator RollCoroutine()
        {
            float amount = 0;
            int indexCell = 0;
            int indexCount = 0;
            
            while (amount < countRoll)
            {
                amount += speedRoll * Time.deltaTime;
                if (amount > Mathf.Floor(amount) + Mathf.Lerp(0f, 1f, indexCell / 5f))
                {
                    for (int i = indexLastCount; i < reels.Count; i++)
                        reels[i].UpdateCell();

                    indexCell = indexCell + 1 < 6 ? indexCell + 1 : 0;
                }

                if (Mathf.FloorToInt(amount) > indexCount)
                {
                    indexCell = 0;
                    indexCount = Mathf.FloorToInt(amount);
                }
            
                indexLastCount = indexCount < countRoll - reels.Count * 5
                    ? 0
                    : (indexCount - (countRoll - reels.Count * 5)) / 5;

                for (int i = 0; i < checkEndSpin.Length; i++)
                    if (i < indexLastCount && !checkEndSpin[i])
                    {
                        reels[i].UpdatePositionReel(0);
                        reels[i].AnimationOut();
                        checkEndSpin[i] = true;
                    }


                for (int i = indexLastCount; i < reels.Count; i++)
                    reels[i].UpdatePositionReel(amount - (Mathf.Floor(amount) + Mathf.Lerp(0f, 1f, indexCell / 5f)));
            
                yield return null;
            }
            
            _isRolling = false;
            foreach (var reel in reels)
            {
                for (int i = 0; i < reels.Count; i++)
                for (int j = 0; j < 4; j++)
                    _board[j, i] = reels[i].GetCellByIndex(j + 1);
            }
            
            payoutController.CheckResult(_board);
        }

        private void ShowWin(List<int> indexLine, List<List<(int x, int y)>> lineWin)
        {
            _coroutineWin = StartCoroutine(ShowWinCoroutine(indexLine, lineWin));
        }

        public void StopShowWin()
        {
            if (_coroutineWin != null)
            {
                StopCoroutine(_coroutineWin);
            }
        }

        IEnumerator ShowWinCoroutine(List<int> indexLine, List<List<(int x, int y)>> lineWin)
        {
            int index = 0;
            
            yield return new WaitForSeconds(0.5f);
            while (true)
            {
                lineController.ShowTargetLine(indexLine[index]);
                yield return ShowLineCoroutine(lineWin[index]);
                index = index + 1 < lineWin.Count ? index + 1 : 0;
            }
        }

        IEnumerator ShowLineCoroutine(List<(int x, int y)> line)
        {
            foreach (var l in line)
            {
                _board[l.x,l.y].ActiveSplash(true);
                _board[l.x,l.y].PlayAnimation(true);
            }

            yield return new WaitForSeconds(1.5f);
            
            foreach (var l in line)
            {
                _board[l.x,l.y].ActiveSplash(false);
                _board[l.x,l.y].PlayAnimation(false);
            }
        }
    }
}
