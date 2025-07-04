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
        [SerializeField] private List<ReelController> reels = new List<ReelController>();
        [SerializeField] private bool[] checkEndSpin;
        [SerializeField] private float speedRoll;
        [SerializeField] private int countRoll;
        [SerializeField] private int indexLastCount;

        private Coroutine _coroutine;

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
        }

        private void OnDisable()
        {
            GameEvent.OnSpin -= Roll;
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

            _isRolling = true;
            for (int i = 0; i < reels.Count; i++)
                checkEndSpin[i] = false;
            
            _coroutine = StartCoroutine(RollCoroutine());
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
        
            foreach (var reel in reels)
                reel.UpdatePositionReel(0);
            
            _isRolling = false;
            payoutController.CheckResult(reels);
        }
    }
}
