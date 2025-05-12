using UnityEngine;

namespace DeliveryContent
{
    public class SkipCounter : MonoBehaviour
    {
        [SerializeField] private GameObject _skipFreeButton;
        [SerializeField] private GameObject _skipAdButton;

        private bool _isFirstSkip = true;

        private void OnEnable()
        {
            Debug.Log("OnEnableDelivery");
            _skipFreeButton.SetActive(_isFirstSkip);
            _skipAdButton.SetActive(!_isFirstSkip);

            _isFirstSkip = false;
        }
    }
}