using System.IO;
using UnityEngine;

namespace SaveSystemContent
{
    public static class LoadDataGame
    {
        public static float LoadFloat(string key, float defaultValue = 0f)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public static int LoadInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public static string LoadString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public static bool LoadBool(string key, bool defaultValue = false)
        {
            int defaultInt = defaultValue ? 1 : 0;
            return PlayerPrefs.GetInt(key, defaultInt) == 1;
        }
        
        private static string GetPath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".json");
        }
        
        public static T LoadJson<T>(string fileName)
        {
            string path = GetPath(fileName);
            if (!File.Exists(path))
            {
                return default;
            }
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }
    }
}