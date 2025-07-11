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

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                //userData = new UserData();
                userData = saveManager.LoadPlayer();
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public UserModel GetUserModel(int index)
        {
            return new UserModel(userData, index);
        }

        public void SaveData()
        {
            saveManager.SavePlayer(userData);
        }
    }
}
