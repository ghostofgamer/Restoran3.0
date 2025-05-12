using InteractableContent;
using PlayerContent;
using TMPro;
using UnityEngine;

namespace RestaurantContent
{
    public class OpenCloseRestaraunt : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private TMP_Text[] _texts;

        private bool _isOpened;

        private void OnEnable()
        {
            _interactableObject.OnAction += SetValue;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= SetValue;
        }

        private void Start()
        {
            Show();
        }

        private void SetValue(PlayerInteraction playerInteraction)
        {
            _isOpened = !_isOpened;
            Show();
        }

        private void Show()
        {
            foreach (var text in _texts)
                text.text = _isOpened ? "Open" : "Close";
        }
    }
}