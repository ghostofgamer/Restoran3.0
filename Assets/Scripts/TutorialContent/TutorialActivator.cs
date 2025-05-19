using Enums;
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
        [SerializeField] private TutorDescriptionUI _tutorDescriptionUI;
        [SerializeField] private TutorialDescription _tutorialDescription;
        [SerializeField] private Tutorial _tutorial;

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

        
        
        
        
        
        
        
        
        private string GetDescriptionText(TutorialType currentType)
        {
            TutorialDescription.Description description =
                System.Array.Find(_tutorialDescription.descriptions, d => d.type == currentType);
            return description != null ? description.text : string.Empty;
        }
    }
}