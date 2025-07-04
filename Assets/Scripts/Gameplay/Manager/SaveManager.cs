using System.IO;
using Data;
using UnityEngine;

namespace Gameplay.Manager
{
    public class SaveManager : MonoBehaviour
    {
        private string SavePath => Application.persistentDataPath + "/player data.json";

        public void SavePlayer(UserData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }

        public UserData LoadPlayer()
        {
            if (!File.Exists(SavePath))
            {
                return null;
            }

            string json = File.ReadAllText(SavePath);
            UserData data = JsonUtility.FromJson<UserData>(json);
            return data;
        }
    }
}
