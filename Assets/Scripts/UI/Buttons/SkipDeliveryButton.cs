using ADSContent;
using DeliveryContent;
using Io.AppMetrica;
using SettingsContent.SoundContent;
using UnityEngine;

namespace UI.Buttons
{
    public class SkipDeliveryButton : AbstractButton
    {
        [SerializeField] private Delivery _delivery;
        [SerializeField] private bool _isAdButton;
        [SerializeField] private ADS _ads;
        [SerializeField] private SkipCounter _skipCounter;

        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();

            if (_isAdButton)
            {
                _ads.ShowRewarded(() => _delivery.SpawnAllItems());
                AppMetrica.ReportEvent("RewardAD", "{\"" + "SkipDeliveryADS" + "\":null}");
            }
            else
            {
                _delivery.SpawnAllItems();
                _skipCounter.SkipFirstActivate();
            }
        }
    }
}