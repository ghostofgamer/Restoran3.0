using UnityEngine;

namespace CustomizationContent
{
    public class DecorCustomization : MonoBehaviour
    {
        [SerializeField]private GameObject[]   _plants;
        [SerializeField] private GameObject[] _paintings;
        [SerializeField]private GameObject[] _stickers;
        [SerializeField]private GameObject[] _shelves;
        [SerializeField]private GameObject[] _others;
        
        public void ChangeActivityPlants(int index)
        {
            _plants[index].SetActive(true);
        }

        public void ChangeActivityPaintings(int index)
        {
            _paintings[index].SetActive(true);
        }

        public void ChangeActivityStickers(int index)
        {
            _stickers[index].SetActive(true);
        }

        public void ChangeActivityShelves(int index)
        {
            _shelves[index].SetActive(true);
        }

        public void ChangeActivityOthers(int index)
        {
            _others[index].SetActive(true);
        }
    }
}