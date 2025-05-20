using System;
using Enums;
using InteractableContent;
using PlayerContent;
using SettingsContent.SoundContent;
using TMPro;
using TutorialContent;
using UnityEngine;

namespace RestaurantContent
{
    public class OpenCloseRestaurant : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private TMP_Text[] _texts;
        [SerializeField] private Tutorial _tutorial;

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
            if ((int)_tutorial.CurrentType < (int)TutorialType.OpenRestaurant)
                return;
            
            if (_tutorial.CurrentType == TutorialType.OpenRestaurant)
                _tutorial.SetCurrentTutorialStage(TutorialType.OpenRestaurant);
            
            SoundPlayer.Instance.PlayButtonClick();
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