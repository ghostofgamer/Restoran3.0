using UnityEngine;

namespace RestaurantContent.TableContent
{
    public class TablesCounter : MonoBehaviour
    {
        [SerializeField] private Table[] _tables;
    
        public Table GetAvailableTable()
        {
            foreach (var table in _tables)
            {
                if (table.gameObject.activeInHierarchy && !table.IsBusy)
                    return table;
            }
            return null;
        }

        public int GetFreeTableCount()
        {
            int freeTableCount = 0;

            foreach (var table in _tables)
            {
                if (table.gameObject.activeInHierarchy && !table.IsBusy)
                    freeTableCount++;
            }

            Debug.Log("GetFreeTableCount " + freeTableCount);
            return freeTableCount;
        }
        
        /*public Table GetAvailableTable()
        {
            foreach (var table in _tables)
            {
                if (!table.IsBusy)
                    return table;
            }
            return null;
        }
        
        public int GetFreeTableCount()
        {
            int freeTableCount = 0;
            
            foreach (var table in _tables)
            {
                if (!table.IsBusy)
                    freeTableCount++;
            }
            
            Debug.Log("GetFreeTableCount " + freeTableCount);
            return freeTableCount;
        }*/
    }
}
