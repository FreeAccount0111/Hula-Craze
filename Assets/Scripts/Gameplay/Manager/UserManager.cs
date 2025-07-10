using System;
using Data;
using UI.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Manager
{
    public class UserManager : MonoBehaviour
    {
        public static UserManager Instance;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private UserData userData;
        [SerializeField] private int indexGame;
        
        public UserModel userModel;

        private void Awake()
        {
            Instance = this;

            //userData = new UserData();
            userData = saveManager.LoadPlayer();
            userModel = new UserModel(userData, indexGame);
        }

        public void SaveData()
        {
            saveManager.SavePlayer(userData, indexGame);
        }
    }
}
