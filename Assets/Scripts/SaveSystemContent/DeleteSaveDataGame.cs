using UnityEngine;

namespace SaveSystemContent
{
    public static class DeleteSaveDataGame 
    {
        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}