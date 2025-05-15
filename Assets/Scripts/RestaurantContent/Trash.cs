using InteractableContent;
using PlayerContent;
using UnityEngine;

namespace RestaurantContent
{
    public class Trash : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;

        private void OnEnable()
        {
            _interactableObject.OnAction += Action;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= Action;
        }

        private void Action(PlayerInteraction playerInteraction)
        {
            if (playerInteraction.CurrentDraggable != null)
                playerInteraction.ThrowItem();
            else
                Debug.Log("Не то в руках или вообще пусто ");
        }

        private void OnTriggerEnter(Collider other)
        {
            Draggable draggable = other.GetComponentInParent<Draggable>();

            if (draggable != null)
            {
                if (!draggable.InHands)
                    draggable.gameObject.SetActive(false);
            }
        }
    }
}