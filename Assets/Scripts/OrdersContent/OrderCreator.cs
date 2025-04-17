using RestorantContent;
using UnityEngine;

namespace OrdersContent
{
    public class OrderCreator : MonoBehaviour
    {
        [SerializeField] private Restorant _restorant;
        [SerializeField] private MenuCounter _menuCounter;
        
        [ContextMenu("CreateOrder")]
        public void CreateOrder()
        {
            Debug.Log("CreateOrder");
            _menuCounter.CategorizeMenuItems();
            
            /*foreach (var menu in _menuCounter.MenuList)
            {
                Debug.Log("menu " + menu);
            }*/
        }
    }
}