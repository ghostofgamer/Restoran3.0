using UnityEngine;

namespace CustomizationContent
{
    public class DecorCustomization : MonoBehaviour
    {
        [SerializeField] private GameObject[] _plants;
        [SerializeField] private GameObject[] _paintings;
        [SerializeField] private GameObject[] _stickers;
        [SerializeField] private GameObject[] _shelves;
        [SerializeField] private GameObject[] _others;

        public void ChangeActivityPlants(int index)
        {
            _plants[index].SetActive(!_plants[index].activeSelf);
        }

        public void ChangeActivityPaintings(int index)
        {
            _paintings[index].SetActive(!_paintings[index].activeSelf);
        }

        public void ChangeActivityStickers(int index)
        {
            _stickers[index].SetActive(!_stickers[index].activeSelf);
        }

        public void ChangeActivityShelves(int index)
        {
            _shelves[index].SetActive(!_shelves[index].activeSelf);
        }

        public void ChangeActivityOthers(int index)
        {
            _others[index].SetActive(!_others[index].activeSelf);
        }
    }
}