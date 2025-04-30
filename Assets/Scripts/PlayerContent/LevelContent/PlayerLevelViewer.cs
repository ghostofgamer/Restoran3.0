using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerContent.LevelContent
{
    public class PlayerLevelViewer : MonoBehaviour
    {
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Image _fillImage;
        [SerializeField] private TMP_Text _levelValueText;

        private void OnEnable()
        {
            _playerLevel.LevelChanged += ShowLevelValue;
            _playerLevel.ExpChanged += ShowExperience;
        }

        private void OnDisable()
        {
            _playerLevel.LevelChanged -= ShowLevelValue;
            _playerLevel.ExpChanged -= ShowExperience;
        }

        private void ShowLevelValue(int levelValue)
        {
            _levelValueText.text = $"Lv.{levelValue}";
        }

        private void ShowExperience(int currentValue, int maxValue)
        {
            _fillImage.fillAmount = (float)currentValue / maxValue;
        }
    }
}