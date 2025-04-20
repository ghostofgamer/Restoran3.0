using TMPro;
using UnityEngine;

namespace RestaurantContent
{
    public class Tray : MonoBehaviour
    {
        [SerializeField] private TMP_Text _indexTable;
        
        public bool IsBusy { get; private set; }

        public void SetBusy(bool value)
        {
            IsBusy = value;
        }

        public void SetIndex(int index)
        {
            _indexTable.gameObject.SetActive(true);
            _indexTable.text = index.ToString();
        }
    }
}