using UnityEngine;

namespace SettingsContent
{
    public class Sensitivity : MonoBehaviour
    {
        private static Sensitivity instance;
        private float sensaValue = 0.5f;

        public static Sensitivity Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject("SenseManager");
                    instance = obj.AddComponent<Sensitivity>();
                }

                return instance;
            }
        }
        
        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }

            // Загрузка сохраненных настроек сенсы
            sensaValue = PlayerPrefs.GetFloat("SensaValue", 0.5f);
        }

        public void SetSensaValue(float value)
        {
            sensaValue = value;
            PlayerPrefs.SetFloat("SensaValue", value);
            PlayerPrefs.Save();
            Debug.Log("Настройки сенсы сохранены: " + value);
        }

        public float GetSensaValue()
        {
            return sensaValue;
        }
    }
}