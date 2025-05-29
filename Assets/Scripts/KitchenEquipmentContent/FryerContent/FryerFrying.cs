using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class FryerFrying : MonoBehaviour
    {
        [SerializeField] private AssemblyFryerTable _assemblyFryerTable;
        [SerializeField] private FryerContainer[] _fryerContainers;
        [SerializeField] private FryerPacking _fryerPacking;
        [SerializeField] private Collider _collider;

        private Coroutine _coroutine;
        private List<FryerTool> _fryerTools = new List<FryerTool>();
        
        public event Action FryCompleted;
        
        public void Fry()
        {
            Debug.Log("Жарить");
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartFry());
        }

        private IEnumerator StartFry()
        {
            _collider.enabled = false;
            
            foreach (var fryerContainer in _fryerContainers)
            {
                Debug.Log("В контейнере пустых мест " + fryerContainer.GetInactiveValue() + " ??? " +
                          fryerContainer.ItemType);

                if (fryerContainer.GetInactiveValue() > 0)
                    SelectFryerTool(fryerContainer.ItemType);
            }

            yield return new WaitForSeconds(4f);
            Debug.Log("закончили жарить ");
            FryCompleted?.Invoke();
            _collider.enabled = true;
        }

        private void SelectFryerTool(ItemType itemType)
        {
            FryerTool fryerTool = _fryerPacking.GetCompatibleFryerTool(itemType);

            if (fryerTool != null)
            {
                int activeValue = fryerTool.GetCountActiveItems();
                Debug.Log("В контейнере активно " + activeValue);

                if (activeValue > 0)
                    fryerTool.MoveFrying();
            }
        }
    }
}