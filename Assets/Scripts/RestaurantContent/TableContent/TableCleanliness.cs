using System.Collections.Generic;
using System.Linq;
using GarbageContent;
using UnityEngine;
using Random = System.Random;

namespace RestaurantContent.TableContent
{
    public class TableCleanliness : MonoBehaviour
    {
        [SerializeField] private GarbagePackage[] _garbagePackages;
        
        private int _maxPollutionLevel = 3;
        private int _pollutionLevel;
        
        public void PolluteTable()
        {
            if (_garbagePackages.Length <= 0) return;

            if (_pollutionLevel >= _maxPollutionLevel) return;
            _pollutionLevel++;

            //это для уборщика список столов мусорных
            // _allBuyerPlaces.AddDirtyBuyerPlace(this);

            Random random = new Random();
            List<GarbagePackage> garbagesTable = _garbagePackages.Where(t => !t.IsActive).ToList();

            if (garbagesTable.Count > 0)
            {
                int randomIndex = random.Next(garbagesTable.Count);

                garbagesTable[randomIndex].SetValue(true);
                Debug.Log("Рандомный индекс " + randomIndex);
            }
        }
        
        public int GetTrashActiveCount()
        {
            int amount = 0;

            foreach (var garbage in _garbagePackages)
            {
                if (garbage.gameObject.activeSelf)
                    amount++;
            }

            return amount;
        }
        
        public void DecreasePollutionLevel()
        {
            if (_pollutionLevel <= 0) return;

            _pollutionLevel--;

            /*if (_pollutionLevel == 0)
                _allBuyerPlaces.RemoveDirtyBuyerPlace(this);*/
            
            Debug.Log("Decreased pollution level: " + _pollutionLevel);
        }

        public void ClearTable()
        {
            if (_pollutionLevel <= 0) return;
            _pollutionLevel = 0;
            DeactivateGarbages();
            
            // _allBuyerPlaces.RemoveDirtyBuyerPlace(this);
        }

        private void DeactivateGarbages()
        {
            foreach (var garbage in _garbagePackages)
                garbage.gameObject.SetActive(false);
        }
    }
}