using UnityEngine;

namespace RestaurantContent.TableContent
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private int _index;
        
        public bool IsBusy { get; private set; }

        public int Index => _index;

        public void SetBusyValue(bool value)
        {
            IsBusy = value;
        }
    }
}