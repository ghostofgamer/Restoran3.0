using UI.Screens.TutorialScreens;
using UnityEngine;

namespace TutorialContent
{
    public class TutorialActivator : MonoBehaviour
    {
        [SerializeField] private NameRestaurantScreen _nameRestaurantScreen;
        [SerializeField] private LookAroundScreen _lookAroundScreen;
        [SerializeField] private MoveScreen _moveScreen;
        
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
    }
}