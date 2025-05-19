using Enums;
using UnityEngine;

namespace TutorialContent
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private TutorialData _tutorialData;
        [SerializeField] private TutorialActivator _tutorialActivator;
        
        public TutorialType CurrentType { get; private set; }

        private void Start()
        {
            /*int savedTutorialStage = PlayerPrefs.GetInt("CurrentTutorialStage", 0);
            CurrentType = (TutorialType)savedTutorialStage;*/


            CurrentType = TutorialType.NameRestaurant;
            CheckCurrentTutorialStage();
        }

        public void SetCurrentTutorialStage(int index)
        {
            TutorialType completedType = (TutorialType)index;
            
            if (completedType == CurrentType)
            {
                TutorialType nextType = GetNextTutorialType(CurrentType);

                if (nextType != CurrentType)
                {
                    CurrentType = nextType;
                    PlayerPrefs.SetInt("CurrentTutorialStage", (int)CurrentType);
                    PlayerPrefs.Save();

                    CheckCurrentTutorialStage();
                }
                else
                {
                    Debug.Log("No more tutorial stages.");
                }
            }
            else
            {
                Debug.LogError("Completed tutorial stage does not match the current stage.");
            }
        }
        
        public void SetCurrentTutorialStage(TutorialType completedType)
        {
            if (completedType == CurrentType)
            {
                TutorialType nextType = GetNextTutorialType(CurrentType);

                if (nextType != CurrentType)
                {
                    CurrentType = nextType;
                    PlayerPrefs.SetInt("CurrentTutorialStage", (int)CurrentType);
                    PlayerPrefs.Save();

                    CheckCurrentTutorialStage();
                }
                else
                {
                    Debug.Log("No more tutorial stages.");
                }
            }
            else
            {
                Debug.LogError("Completed tutorial stage does not match the current stage.");
            }
        }

        private TutorialType GetNextTutorialType(TutorialType currentType)
        {
            TutorialType[] allTypes = (TutorialType[])System.Enum.GetValues(typeof(TutorialType));

            int currentIndex = System.Array.IndexOf(allTypes, currentType);

            if (currentIndex < allTypes.Length - 1)
                return allTypes[currentIndex + 1];

            return currentType;
        }

        private void CheckCurrentTutorialStage()
        {
            switch (CurrentType)
            {
                case TutorialType.NameRestaurant:
                    Debug.Log("Current Tutorial Stage: NameRestaurant");
                   _tutorialActivator.ActivateNameRestaurant();
                    break;
                case TutorialType.LookAround:
                    Debug.Log("Current Tutorial Stage: LookAround");
                    _tutorialActivator.ActivateLookAround();
                    break;
                case TutorialType.MoveAround:
                    Debug.Log("Current Tutorial Stage: MoveAround");
                    _tutorialActivator.ActivateMove();
                    break;
                case TutorialType.TakeBoxBuns:
                    Debug.Log("Current Tutorial Stage: TakeBoxBuns");
                    // Логика для этапа TakeBoxBuns
                    break;
                case TutorialType.PutBunsAssemblyTable:
                    Debug.Log("Current Tutorial Stage: PutBunsAssemblyTable");
                    // Логика для этапа PutBunsAssemblyTable
                    break;
                case TutorialType.ThrowEmptyBoxInTrash:
                    Debug.Log("Current Tutorial Stage: ThrowEmptyBoxInTrash");
                    // Логика для этапа ThrowEmptyBoxInTrash
                    break;
                case TutorialType.TakeBoxBurgerPackages:
                    Debug.Log("Current Tutorial Stage: TakeBoxBurgerPackages");
                    // Логика для этапа TakeBoxBurgerPackages
                    break;
                case TutorialType.PutPackagesAssemblyTable:
                    Debug.Log("Current Tutorial Stage: PutPackagesAssemblyTable");
                    // Логика для этапа PutPackagesAssemblyTable
                    break;
                case TutorialType.OrderBurgerPatties:
                    Debug.Log("Current Tutorial Stage: OrderBurgerPatties");
                    // Логика для этапа OrderBurgerPatties
                    break;
                case TutorialType.SkipDelivery:
                    Debug.Log("Current Tutorial Stage: SkipDelivery");
                    // Логика для этапа SkipDelivery
                    break;
                case TutorialType.TakeBoxesOutside:
                    Debug.Log("Current Tutorial Stage: TakeBoxesOutside");
                    // Логика для этапа TakeBoxesOutside
                    break;
                case TutorialType.PutRawCutletInContainer:
                    Debug.Log("Current Tutorial Stage: PutRawCutletInContainer");
                    // Логика для этапа PutRawCutletInContainer
                    break;
                default:
                    Debug.Log("Unknown Tutorial Stage");
                    break;
            }
        }
    }
}