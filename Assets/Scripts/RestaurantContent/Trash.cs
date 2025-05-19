using Enums;
using InteractableContent;
using PlayerContent;
using TutorialContent;
using UnityEngine;

namespace RestaurantContent
{
    public class Trash : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private Tutorial _tutorial;

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
                {
                    draggable.gameObject.SetActive(false);

                    if (_tutorial.CurrentType == TutorialType.ThrowEmptyBoxInTrash)
                        _tutorial.SetCurrentTutorialStage(TutorialType.ThrowEmptyBoxInTrash);
                }
            }
        }
    }
}