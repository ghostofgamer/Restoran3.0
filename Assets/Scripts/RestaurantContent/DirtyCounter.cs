using System;
using System.Collections.Generic;
using RestaurantContent.TableContent;
using UnityEngine;

namespace RestaurantContent
{
    public class DirtyCounter : MonoBehaviour
    {
        [SerializeField] private List<TableCleanliness> _allTables = new List<TableCleanliness>();

        private List<TableCleanliness> _dirtyTables = new List<TableCleanliness>();

        public event Action DirtyTableAdded;

        public List<TableCleanliness> DirtyTables => _dirtyTables;

        private void OnEnable()
        {
            foreach (var table in _allTables)
            {
                table.TablePolluted += AddDirtyTable;
                table.TableCleaned += RemoveDirtyTable;
            }
        }

        private void OnDisable()
        {
            foreach (var table in _allTables)
            {
                table.TablePolluted -= AddDirtyTable;
                table.TableCleaned -= RemoveDirtyTable;
            }
        }

        public void AddDirtyTable(TableCleanliness tableCleanliness)
        {
            if (!_dirtyTables.Contains(tableCleanliness))
            {
                Debug.Log("Добавили грязный стол");
                _dirtyTables.Add(tableCleanliness);
                DirtyTableAdded?.Invoke();
            }
            else
            {
                Debug.Log("Стол уже в списке");
            }
        }

        public void RemoveDirtyTable(TableCleanliness tableCleanliness)
        {
            Debug.Log("удалили из списка грязных столов");
            _dirtyTables.Remove(tableCleanliness);
        }

        public TableCleanliness GetDirtyTable()
        {
            if (_dirtyTables.Count > 0)
            {
                Debug.Log("получи грязный стол");
                return _dirtyTables[0];
            }
            else
            {
                Debug.Log("нету грязных столов");
                return null;
            }
        }
    }
}