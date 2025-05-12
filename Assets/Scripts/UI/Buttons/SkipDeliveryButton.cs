using ADSContent;
using DeliveryContent;
using UnityEngine;

namespace UI.Buttons
{
    public class SkipDeliveryButton : AbstractButton
    {
        [SerializeField] private Delivery _delivery;
        [SerializeField] private bool _isAdButton;
        [SerializeField] private ADS _ads;

        public override void OnClick()
        {
            if (_isAdButton)
                _ads.ShowRewarded(() => _delivery.SpawnAllItems());
            else
                _delivery.SpawnAllItems();
        }
    }
}