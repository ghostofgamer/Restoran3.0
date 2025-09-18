using System.IO;
using UnityEngine;

namespace SaveSystemContent
{
    public static class SaveDataGame
    {
        public static void SaveFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key,value);
            PlayerPrefs.Save();
        }
        
        public static void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }
        
        public static void SaveString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }
        
        public static void SaveBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            PlayerPrefs.Save();
        }
        
        private static string GetPath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".json");
        }
        
        public static void SaveJson<T>(string fileName, T data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(GetPath(fileName), json);
        }
    }
}
