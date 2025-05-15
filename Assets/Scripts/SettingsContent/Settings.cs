using UnityEngine;
using UnityEngine.UI;

namespace SettingsContent
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private Slider _sliderSensa; 
        
        void Start()
        {
            // Загрузка сохраненных настроек сенсы
            float savedSensaValue = Sensitivity.Instance.GetSensaValue();
            _sliderSensa.value = savedSensaValue;
            Debug.Log("Загруженные настройки сенсы: " + savedSensaValue);
        }
        
        public void SetValueSound(bool value)
        {
            Debug.Log("ЗВУК " + value);
        }
        
        public void SetValueMusic(bool value)
        {
            Debug.Log("Музыка " + value);
        }
        
        public void SetValueSensa(float value)
        {
            // Сохранение настроек сенсы
            Sensitivity.Instance.SetSensaValue(value);
        }
    }
}