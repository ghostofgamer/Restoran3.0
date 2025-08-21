using System;
using AttentionHintContent;
using Enums;
using I2.Loc;
using InteractableContent;
using PlayerContent;
using RestaurantContent.MenuContent;
using SettingsContent;
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
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Material _openMaterial;
        [SerializeField] private Material _closeMaterial;
        [SerializeField] private Renderer _colorObject;
        [SerializeField] private Color _openColor;
        [SerializeField] private Color _closeColor;
        [SerializeField] private LanguageChanger _languageChanger;
        [SerializeField] private MenuCounter _menuCounter;
        [SerializeField] private GameObject _pointer;

        public bool IsOpened { get; private set; }

        public event Action<bool> OpenedChanged;

        private void OnEnable()
        {
            _interactableObject.OnAction += SetValue;
            _languageChanger.LanguageChanged += ChangeLanguage;
            _tutorial.TutorCompleted += ActivatePointer;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= SetValue;
            _languageChanger.LanguageChanged += ChangeLanguage;
            _tutorial.TutorCompleted -= ActivatePointer;
        }

        private void Start()
        {
            Show();
            SetValuePointer(true);
        }

        private void ActivatePointer()
        {
            if (!IsOpened)
                _pointer.SetActive(true);
        }

        private void SetValuePointer(bool value)
        {
            Debug.Log("!!!!SetValuePointer " + value);

            if (_tutorial.CurrentType >= TutorialType.TutorCompleted)
                _pointer.SetActive(value);
        }

        private void SetValue(PlayerInteraction playerInteraction)
        {
            if ((int)_tutorial.CurrentType < (int)TutorialType.OpenRestaurant)
            {
                AttentionHintActivator.Instance.ShowHint(
                    LocalizationManager.GetTermTranslation("You are not ready to open yet!"));
                return;
            }

            if (_tutorial.CurrentType == TutorialType.OpenRestaurant)
                _tutorial.SetCurrentTutorialStage(TutorialType.OpenRestaurant);

            if (_menuCounter.MenuList.Count <= 0)
            {
                AttentionHintActivator.Instance.ShowHint(
                    LocalizationManager.GetTermTranslation("MenuEmpty"));
                return;
            }

            SoundPlayer.Instance.PlayButtonClick();
            IsOpened = !IsOpened;
            SetValuePointer(!IsOpened);
            OpenedChanged?.Invoke(IsOpened);
            Show();
        }

        private void Show()
        {
            if (IsOpened)
            {
                _text.text = LocalizationManager.GetTermTranslation("OPEN");
                _text.color = _openColor;
                _colorObject.material = _openMaterial;
            }
            else
            {
                _text.text = LocalizationManager.GetTermTranslation("CLOSED");
                _text.color = _closeColor;
                _colorObject.material = _closeMaterial;
            }
        }

        private void ChangeLanguage()
        {
            if (IsOpened)
                _text.text = LocalizationManager.GetTermTranslation("OPEN");
            else
                _text.text = LocalizationManager.GetTermTranslation("CLOSED");
        }
    }
}