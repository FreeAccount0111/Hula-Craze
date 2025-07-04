using System;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Manager
{
    public class UserManager : MonoBehaviour
    {
        public static UserManager Instance;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private UserData userData;
        public UserData UserData => userData;

        private void Awake()
        {
            Instance = this;
            
            userData = saveManager.LoadPlayer() ?? new UserData();
        }
    }
}
