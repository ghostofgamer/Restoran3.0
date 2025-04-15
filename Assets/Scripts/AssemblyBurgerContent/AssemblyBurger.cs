using InteractableContent;
using UnityEngine;

namespace AssemblyBurgerContent
{
    public class AssemblyBurger : MonoBehaviour
    {
        [SerializeField] private BurgerBoard _burgerBoard;
        [SerializeField] private BurgerIngridientSpawner _burgerIngridientSpawner;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("HIT " + hit.collider.name);

                    ItemContainer selectedContainer = hit.collider.GetComponent<ItemContainer>();

                    if (selectedContainer != null)
                    {
                        if (selectedContainer.IsAdditionalItemsContainer)
                        {
                            int[] activeItems = selectedContainer.GetActivePositions();

                            for (int i = 0; i < activeItems.Length; i++)
                            {
                                Debug.Log("Active count in sub-array " + i + ": " + activeItems[i]);
                            }
                        }
                        else
                        {
                            int activeItems = selectedContainer.GetActiveItemsValue();
                            Debug.Log("activeItems: " + activeItems);
                            Debug.Log("selectedContainer: " + selectedContainer.name);
                            Debug.Log("ItemType: " + selectedContainer.CurrentItemContainer);

                            HandleContainerSelection(activeItems, selectedContainer.CurrentItemContainer);
                        }
                    }
                }
            }
        }

        private void HandleContainerSelection(int value, ItemType type)
        {
            Item item = _burgerIngridientSpawner.SpawnItem(type);
            item.transform.position = _burgerBoard.CenterPosition.position;
            item.transform.rotation = _burgerBoard.CenterPosition.rotation;
            item.transform.localScale = new Vector3(3,3,3);
            item.gameObject.SetActive(true);
        }
    }
}