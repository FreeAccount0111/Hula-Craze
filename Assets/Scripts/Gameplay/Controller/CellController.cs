using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Controller
{
    public enum SymbolType
    {
        Nine = 0,
        Ten = 1,
        J = 2,
        Q = 3,
        K = 4,
        A = 5,
        Buffalo = 6,
        Pig = 7,
        Panther = 8,
        Crocodile = 9,
        Gorilla = 10,
        Wild = 11,
        Scatter = 12,
    }
    
    public class CellController : MonoBehaviour
    {
        [SerializeField] private SymbolType symbolType;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject splash;
        [SerializeField] private SpriteRenderer[] renderer;

        public SymbolType SymbolType => symbolType;

        private void Awake()
        {
            renderer = GetComponentsInChildren<SpriteRenderer>();
        }

        public void ActiveSplash(bool en)
        { 
            splash.SetActive(en);
            foreach (var r in renderer)
                r.sortingLayerName = en ? "Target" : "Default";
        }
        public void PlayAnimation(bool en) => animator.enabled = en;
    }
}
