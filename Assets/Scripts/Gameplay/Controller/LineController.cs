using System;
using System.Collections.Generic;
using UI.Interfaces;
using UI.Views;
using UnityEngine;

namespace Gameplay.Controller
{
    public class LineController : MonoBehaviour,ILineView
    {
        public static LineController Instance;
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject transLine;
        [SerializeField] private List<GameObject> lines = new List<GameObject>();

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        public void ShowTargetLine(int index)
        {
            ShowLines();
            for (int i = 0; i < lines.Count; i++)
                lines[i].gameObject.SetActive(i == index);
        }

        public void UpdateLine(int line)
        {
            for (int i = 0; i < lines.Count; i++)
                lines[i].gameObject.SetActive(i < line);
        }

        public void ShowLines()
        {
            background.SetActive(true);
            transLine.SetActive(true);
        }

        public void HideLines()
        {
            background.SetActive(false);
            transLine.SetActive(false);
        }
    }
}
