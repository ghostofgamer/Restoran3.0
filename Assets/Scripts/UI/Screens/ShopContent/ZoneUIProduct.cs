using UnityEngine;

namespace UI.Screens.ShopContent
{
    public class ZoneUIProduct : MonoBehaviour
    {
        [SerializeField] private GameObject _ownedObjectInfo;
        [SerializeField] private GameObject _requaredObjectInfo;
        [SerializeField] private GameObject _buyObjectInfo;
        [SerializeField] private int _levelOpened;
        [SerializeField] private GameObject _wallZone;

        private bool _isOwned;

        public void Init(int levelPlayer)
        {
            _isOwned = IsBuyed();
            _requaredObjectInfo.SetActive(levelPlayer < _levelOpened && !_isOwned);
            _ownedObjectInfo.SetActive(levelPlayer >= _levelOpened && _isOwned);
            _buyObjectInfo.SetActive(levelPlayer >= _levelOpened && !_isOwned);
        }

        public bool IsBuyed()
        {
            return PlayerPrefs.GetInt("Zona" + _levelOpened, 0) > 0;
        }

        public void Buy()
        {
            _isOwned = true;
            _ownedObjectInfo.SetActive(true);
            _buyObjectInfo.SetActive(false);
            _wallZone.gameObject.SetActive(false);
            PlayerPrefs.SetInt("Zona" + _levelOpened, 1);
        }
    }
}