using System;
using Gameplay.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.Manager
{
    public class LoadSceneManager : MonoBehaviour,ILoadScene
    {
        public static LoadSceneManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadScene(string nameScene)
        {
            SceneManager.LoadSceneAsync(nameScene);
        }
    }
}
