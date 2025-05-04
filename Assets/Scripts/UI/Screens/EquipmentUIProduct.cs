using TMPro;
using UnityEngine;

namespace UI.Screens
{
    public class EquipmentUIProduct : MonoBehaviour
    {
        public const string Equipment = "Equipment";

        [SerializeField] private GameObject _ownedObjectInfo;
        [SerializeField] private GameObject _requaredObjectInfo;
        [SerializeField] private GameObject _buyObjectInfo;
        [SerializeField] private int _levelOpened;
        [SerializeField] private GameObject _equipment;
        [SerializeField] private TMP_Text _requaredText;

        private bool _isOwned;

        private void Start()
        {
            _requaredText.text = $"Requared is {_levelOpened} level";
        }

        public void Init(int levelPlayer)
        {
            _isOwned = IsBuyed();
            _requaredObjectInfo.SetActive(levelPlayer < _levelOpened && !_isOwned);
            _ownedObjectInfo.SetActive(levelPlayer >= _levelOpened && _isOwned);
            _buyObjectInfo.SetActive(levelPlayer >= _levelOpened && !_isOwned);
        }
        
        public void Buy()
        {
            _isOwned = true;
            _ownedObjectInfo.SetActive(true);
            _buyObjectInfo.SetActive(false);
            _equipment.gameObject.SetActive(true);
            PlayerPrefs.SetInt(Equipment + _levelOpened, 1);
        }

        public bool IsBuyed()
        {
            return PlayerPrefs.GetInt(Equipment + _levelOpened, 0) > 0;
        }
    }
}