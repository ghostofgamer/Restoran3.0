using ClientsContent;
using RestaurantContent.TableContent;
using RestorantContent;
using UnityEngine;

namespace RestaurantContent
{
    public class Restaurant : MonoBehaviour
    {
        [SerializeField] private MenuCounter _menuCounter;
        [SerializeField] private QueueCashRegister _queueCashRegister;
        [SerializeField] private TablesCounter _tablesCounter;

        public void AcceptOrder()
        {
            _queueCashRegister.ClientFinishedOrder();
        }
    }
}