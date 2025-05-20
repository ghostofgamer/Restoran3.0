using System.Collections;
using Enums;
using InputContent;
using ItemContent;
using SoContent;
using UI.Screens.TutorialScreens;
using UnityEngine;

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
        [SerializeField] private TutorDescriptionUI _tutorDescriptionUI;
        [SerializeField] private TutorialDescription _tutorialDescription;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private LookAroundEventTrigger _lookAroundEventTrigger;
        [SerializeField] private GameObject _joystick;
        [SerializeField] private GameObject _touchShopImage;
        [SerializeField] private GameObject _touchSkipImage;
        [SerializeField] private BoxesCounter _boxesCounter;

        public void ActivateNameRestaurant()
        {
            _nameRestaurantScreen.OpenScreen();
        }

        public void ActivateLookAround()
        {
            _lookAroundScreen.OpenScreen();
        }

        public void ActivateMove()
        {
            _moveScreen.OpenScreen();
        }

        public void TakeBunBox()
        {
            Debug.Log("TakeBunBox");
            bunObject.gameObject.SetActive(true);
            bunObject.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }

        public void PutBunsAssemblyTableBunBox()
        {
            Debug.Log("PutBunsAssemblyTableBunBox");
            _assemblyTable.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }

        public void ThrowEmptyBoxInTrash()
        {
            Debug.Log("ThrowEmptyBoxInTrash");
            _assemblyTable.DeactivateTutorPoint();
            _trash.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }

        public void TakeBoxBurgerPackages()
        {
            Debug.Log("TakeBoxBurgerPackages");
            _trash.DeactivateTutorPoint();
            _burgerPackageBox.gameObject.SetActive(true);
            _burgerPackageBox.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }

        public void PutPackagesAssemblyTable()
        {
            Debug.Log("PutPackagesAssemblyTable");
            // _burgerPackageBox.DeactivateTutorPoint();
            _assemblyTable.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }

        public void OrderBurgerPatties()
        {
            Debug.Log("OrderBurgerPatties");
            _assemblyTable.DeactivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
            // _playerInput.enabled = false;
            _lookAroundEventTrigger.gameObject.SetActive(false);
            _joystick.SetActive(false);
            _touchShopImage.SetActive(true);
        }

        public void SkipDelivery()
        {
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
            _touchSkipImage.SetActive(true);
        }

        public void TakeBoxesOutside()
        {
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
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
            _rawCutletContainer.ActivateTutorPoint();
        }
        
        public void  TakeRawCutletInTrayPlayer()
        {
            _rawCutletContainer.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }
        
        public void PutCutletsOnGrill()
        {
            _rawCutletContainer.DeactivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
            _grillTutorObject.ActivateTutorPoint();
        }
        
        public void FryCutletGrill()
        {
            _grillTutorObject.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }
        
        public void TakeWellCutlet()
        {
            _grillTutorObject.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }
        
        public void PutWellCutletToContainer()
        {
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
            _grillTutorObject.DeactivateTutorPoint();
            _assemblyTable.ActivateTutorPoint();
        }
        
        public void LetsMakeFirstBurger()
        {
            _assemblyTable.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType));
        }
        
        private string GetDescriptionText(TutorialType currentType)
        {
            TutorialDescription.Description description =
                System.Array.Find(_tutorialDescription.descriptions, d => d.type == currentType);
            return description != null ? description.text : string.Empty;
        }
    }
}