using UnityEngine;

namespace RestaurantContent.TableContent
{
    public class Table : MonoBehaviour
    {
        public bool IsBusy { get; private set; }

        public void SetBusyValue(bool value)
        {
            IsBusy = value;
        }
    }
}