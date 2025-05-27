using PlayerContent.LevelContent;
using UnityEngine;

namespace DisableInterContent
{
    public class DisablerInter : MonoBehaviour
    {
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private GameObject _buttonOpenDisableInterScreen;

        private void OnEnable()
        {
            _playerLevel.LevelChanged += SetValue;
        }

        private void OnDisable()
        {
            _playerLevel.LevelChanged -= SetValue;
        }

        private void Start()
        {
            SetValue(_playerLevel.CurrentLevel);
        }

        private void SetValue(int level)
        {
            _buttonOpenDisableInterScreen.SetActive(level >= 2);
        }
    }
}