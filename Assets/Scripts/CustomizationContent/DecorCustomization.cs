using CustomizationContent.SavesCustomization;
using UnityEngine;

namespace CustomizationContent
{
    public class DecorCustomization : MonoBehaviour
    {
        [SerializeField]private SaveDecorCustomization _saveDecorCustomization;
        [SerializeField] private GameObject[] _plants;
        [SerializeField] private GameObject[] _paintings;
        [SerializeField] private GameObject[] _stickers;
        [SerializeField] private GameObject[] _shelves;
        [SerializeField] private GameObject[] _others;
        
        public GameObject[] Plants => _plants;
        public GameObject[] Paintings => _paintings;
        public GameObject[] Stickers => _stickers;
        public GameObject[] Shelves => _shelves;
        public GameObject[] Others => _others;

        public void ChangeActivityPlants(int index)
        {
            _plants[index].SetActive(!_plants[index].activeSelf);
            _saveDecorCustomization.Save();
        }

        public void ChangeActivityPaintings(int index)
        {
            _paintings[index].SetActive(!_paintings[index].activeSelf);
            _saveDecorCustomization.Save();
        }

        public void ChangeActivityStickers(int index)
        {
            _stickers[index].SetActive(!_stickers[index].activeSelf);
            _saveDecorCustomization.Save();
        }

        public void ChangeActivityShelves(int index)
        {
            _shelves[index].SetActive(!_shelves[index].activeSelf);
            _saveDecorCustomization.Save();
        }

        public void ChangeActivityOthers(int index)
        {
            _others[index].SetActive(!_others[index].activeSelf);
            _saveDecorCustomization.Save();
        }
    }
}