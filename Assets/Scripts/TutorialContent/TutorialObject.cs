using Enums;
using UnityEngine;
using WrightAngle.Waypoint;

namespace TutorialContent
{
    public class TutorialObject : MonoBehaviour
    {
        [SerializeField] private TutorialType _itemType;
        [SerializeField] private WaypointTarget _waypointTarget;
        [SerializeField] private GameObject _rawTutor;

        public TutorialType ItemType => _itemType;

        public void ActivateTutorPoint()
        {
            Debug.Log("ActivateTutorPoint 1");
            _waypointTarget.ActivateWaypoint();
            Debug.Log("ActivateTutorPoint 3");
            _rawTutor.SetActive(true);
        }

        public void DeactivateTutorPoint()
        {
            _waypointTarget.DeactivateWaypoint();
            _rawTutor.SetActive(false);
        }
    }
}