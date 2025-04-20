using UnityEngine;
using UnityEngine.UIElements;

namespace RestaurantContent.TableContent
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private int _index;
        [SerializeField] private Transform _clientPosition;

        public Transform ClientPosition => _clientPosition;
        
        public bool IsBusy { get; private set; }

        public int Index => _index;

        public void SetBusyValue(bool value)
        {
            IsBusy = value;
        }
    }
}