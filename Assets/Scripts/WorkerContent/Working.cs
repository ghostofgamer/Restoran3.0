using Enums;
using RestaurantContent;
using RestaurantContent.TableContent;
using UnityEngine;

namespace WorkerContent
{
    public class Working : MonoBehaviour
    {
        [SerializeField] private DirtyCounter _dirtyCounter;
        [SerializeField] private Workers _workers;

        private void OnEnable()
        {
            _dirtyCounter.DirtyTableAdded += CallCleaner;
        }

        private void OnDisable()
        {
            _dirtyCounter.DirtyTableAdded -= CallCleaner;
        }

        private void CallCleaner()
        {
            TableCleanliness dirtyTable = _dirtyCounter.GetDirtyTable();

            Worker cleaner = _workers.GetCleaner(WorkerType.Cleaner);

            if (cleaner != null)
            {
                
            }
            else
            {
                Debug.Log("туту Null уборщик");
                return;
            }
            
            if (dirtyTable != null)
            {
                
            }
            else
            {
                Debug.Log("туту Null стол");
                return;
            }
        }
    }
}