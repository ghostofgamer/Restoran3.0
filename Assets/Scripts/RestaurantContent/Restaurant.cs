using ClientsContent;
using UnityEngine;

namespace RestorantContent
{
    public class Restaurant : MonoBehaviour
    {
        [SerializeField] private MenuCounter _menuCounter;
        [SerializeField] private QueueCashRegister _queueCashRegister;
    }
}