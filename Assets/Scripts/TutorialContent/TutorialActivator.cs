using System.Collections;
using Enums;
using InputContent;
using ItemContent;
using SoContent;
using UI.Screens;
using UI.Screens.TutorialScreens;
using UnityEngine;
using UnityEngine.UI;

namespace TutorialContent
{
    public class TutorialActivator : MonoBehaviour
    {
        [SerializeField] private NameRestaurantScreen _nameRestaurantScreen;
        [SerializeField] private LookAroundScreen _lookAroundScreen;
        [SerializeField] private MoveScreen _moveScreen;
        [SerializeField] private TutorialObject bunObject;
        [SerializeField] private TutorialObject _assemblyTable;
        [SerializeField] private TutorialObject _trash;
        [SerializeField] private TutorialObject _burgerPackageBox;
        [SerializeField] private TutorialObject _rawCutletContainer;
        [SerializeField] private TutorialObject _grillTutorObject;
        [SerializeField] private TutorialObject _openCloseRest;
        [SerializeField] private TutorialObject _cashRegister;
        [SerializeField] private TutorialObject _tableFirstClient;
        [SerializeField] private TutorDescriptionUI _tutorDescriptionUI;
        [SerializeField] private TutorialDescription _tutorialDescription;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private LookAroundEventTrigger _lookAroundEventTrigger;
        [SerializeField] private GameObject _joystick;
        [SerializeField] private GameObject _touchShopImage;
        [SerializeField] private GameObject _touchSkipImage;
        [SerializeField] private BoxesCounter _boxesCounter;
        [SerializeField] private Button _closeCashRegister;
        [SerializeField] private AssemblyBurgerScreen _assemblyBurgerScreen;

        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _dailyReward;
        [SerializeField] private Button _fortune;

        public void ActivateNameRestaurant()
        {
            SetValueButtonTopUI(false);
            _nameRestaurantScreen.OpenScreen();
        }

        public void ActivateLookAround()
        {
            SetValueButtonTopUI(false);
            _lookAroundScreen.OpenScreen();
        }

        public void ActivateMove()
        {
            SetValueButtonTopUI(false);
            _moveScreen.OpenScreen();
        }

        public void TakeBunBox()
        {
            SetValueButtonTopUI(false);
            Debug.Log("TakeBunBox");
            bunObject.gameObject.SetActive(true);
            bunObject.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }

        public void PutBunsAssemblyTableBunBox()
        {
            SetValueButtonTopUI(false);
            Debug.Log("PutBunsAssemblyTableBunBox");
            _assemblyTable.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }

        public void ThrowEmptyBoxInTrash()
        {
            SetValueButtonTopUI(false);
            Debug.Log("ThrowEmptyBoxInTrash");
            _assemblyTable.DeactivateTutorPoint();
            _trash.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }

        public void TakeBoxBurgerPackages()
        {
            SetValueButtonTopUI(false);
            Debug.Log("TakeBoxBurgerPackages");
            _trash.DeactivateTutorPoint();
            _burgerPackageBox.gameObject.SetActive(true);
            _burgerPackageBox.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }

        public void PutPackagesAssemblyTable()
        {
            SetValueButtonTopUI(false);
            Debug.Log("PutPackagesAssemblyTable");
            // _burgerPackageBox.DeactivateTutorPoint();
            _assemblyTable.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }

        public void OrderBurgerPatties()
        {
            SetValueButtonTopUI(false);
            Debug.Log("OrderBurgerPatties");
            _shopButton.interactable = true;
            _assemblyTable.DeactivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
            // _playerInput.enabled = false;
            _lookAroundEventTrigger.gameObject.SetActive(false);
            _joystick.SetActive(false);
            _touchShopImage.SetActive(true);
        }

        public void SkipDelivery()
        {
            SetValueButtonTopUI(false);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
            _touchSkipImage.SetActive(true);
        }

        public void TakeBoxesOutside()
        {
            SetValueButtonTopUI(false);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
            _lookAroundEventTrigger.gameObject.SetActive(true);
            _joystick.SetActive(true);
            _touchSkipImage.SetActive(false);
            StartCoroutine(SearchBoxOutside());
        }

        private IEnumerator SearchBoxOutside()
        {
            yield return new WaitForSeconds(0.15f);
            ItemBasket basketRawCutlet = _boxesCounter.GetItemBasketByType(ItemType.RawCutlet);
            basketRawCutlet.GetComponent<TutorialObject>().ActivateTutorPoint();
        }

        public void PutRawCutletInContainer()
        {
            SetValueButtonTopUI(false);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
            _rawCutletContainer.ActivateTutorPoint();
        }
        
        public void  TakeRawCutletInTrayPlayer()
        {
            SetValueButtonTopUI(false);
            _rawCutletContainer.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }
        
        public void PutCutletsOnGrill()
        {
            SetValueButtonTopUI(false);
            _rawCutletContainer.DeactivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
            _grillTutorObject.ActivateTutorPoint();
        }
        
        public void FryCutletGrill()
        {
            SetValueButtonTopUI(false);
            _grillTutorObject.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }
        
        public void TakeWellCutlet()
        {
            SetValueButtonTopUI(false);
            _grillTutorObject.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }
        
        public void PutWellCutletToContainer()
        {
            SetValueButtonTopUI(false);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
            _grillTutorObject.DeactivateTutorPoint();
            _assemblyTable.ActivateTutorPoint();
        }
        
        public void LetsMakeFirstBurger()
        {
            SetValueButtonTopUI(false);
            _assemblyTable.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }
        
        public void LetsSetPrice()
        {
            _assemblyBurgerScreen.CloseScreen();
            SetValueButtonTopUI(false);
            _shopButton.interactable = true;
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
            _lookAroundEventTrigger.gameObject.SetActive(false);
            _joystick.SetActive(false);
            _touchShopImage.SetActive(true);
        }
        
        public void OpenRestaurant()
        {
            SetValueButtonTopUI(false);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
            _lookAroundEventTrigger.gameObject.SetActive(true);
            _joystick.SetActive(true);
            _openCloseRest.ActivateTutorPoint();
        }
        
        public void TakeFirstOrder()
        {
            SetValueButtonTopUI(false);
            _closeCashRegister.interactable = false;
            _openCloseRest.DeactivateTutorPoint();
            _cashRegister.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }
        
        public void CleanTable()
        {
            SetValueButtonTopUI(false);
            _cashRegister.DeactivateTutorPoint();
            _tableFirstClient.ActivateTutorPoint();
            _closeCashRegister.interactable = true;
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }
        
        public void TutorCompleted()
        {
            SetValueButtonTopUI(true);
            _tableFirstClient.DeactivateTutorPoint();
            _tutorDescriptionUI.StartCompleted(GetDescriptionText(_tutorial.CurrentType));
        }
        
        private string GetDescriptionText(TutorialType currentType)
        {
            TutorialDescription.Description description =
                System.Array.Find(_tutorialDescription.descriptions, d => d.type == currentType);
            return description != null ? description.text : string.Empty;
        }

        private void SetValueButtonTopUI(bool value)
        {
            _shopButton.interactable = value;
            _dailyReward.interactable = value;
            _fortune.interactable = value;
        }
    }
}