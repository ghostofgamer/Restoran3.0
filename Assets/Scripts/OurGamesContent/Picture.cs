using InteractableContent;
using Io.AppMetrica;
using MetricsContent;
using PlayerContent;
using UI.Screens;
using UnityEngine;

namespace OurGamesContent
{
    public class Picture : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private OurGamesScreen _ourGamesScreen;
        
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
            OurGamesMetrics.ReportClickPicture();
            _ourGamesScreen.OpenScreen();
        }
    }
}
