using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace Gameplay.Controller
{
    public class ReelController : MonoBehaviour
    {
        [SerializeField] private List<CellController> cells = new List<CellController>();
        [SerializeField] private Transform content;
        [SerializeField] private Transform posTop, posBottom;

        public CellController GetCellByIndex(int index)
        {
            return cells[index];
        }
        public void FillReel()
        {
            for (int i = 0; i < 6; i++)
            {
                var newCell = GetRandomCell();
                newCell.transform.SetParent(content);
                cells.Add(newCell);
            }

            for (int i = 0; i < 6; i++)
            {
                cells[i].transform.position = Vector3.Lerp(posBottom.position, posTop.position, i / 5f);
            }
        }

        private CellController GetRandomCell()
        {
            return ObjectPool.Instance
                .Get(ObjectPool.Instance.cells[UnityEngine.Random.Range(0, ObjectPool.Instance.cells.Count)])
                .GetComponent<CellController>();
        }

        public void UpdatePositionReel(float amount)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].transform.position = Vector3.Lerp(posBottom.position, posTop.position, i / 5f)
                                              + Vector3.down * Vector2.Distance(posTop.position, posBottom.position) * amount;
            }
        }

        public void UpdateCell()
        {
            var firstCell = cells[0];
            ObjectPool.Instance.Return(firstCell.gameObject,true);
            cells.Remove(firstCell);

            var newCell = GetRandomCell();
            newCell.transform.SetParent(content);
            cells.Add(newCell);
        }

        public void AnimationOut()
        {
            content.DOLocalMoveY(-0.75f, 0.25f).OnComplete(() =>
            {
                content.DOLocalMoveY(0, 0.25f);
            });
        }
    }
}
