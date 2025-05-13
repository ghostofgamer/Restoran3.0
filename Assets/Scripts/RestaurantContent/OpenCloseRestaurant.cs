using System;
using InteractableContent;
using PlayerContent;
using TMPro;
using UnityEngine;

namespace RestaurantContent
{
    public class OpenCloseRestaurant : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private TMP_Text[] _texts;

        public bool IsOpened { get; private set; }

        public event Action<bool> OpenedChanged;

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
            IsOpened = !IsOpened;
            OpenedChanged?.Invoke(IsOpened);
            Show();
        }

        private void Show()
        {
            foreach (var text in _texts)
                text.text = IsOpened ? "Open" : "Close";
        }
    }
}