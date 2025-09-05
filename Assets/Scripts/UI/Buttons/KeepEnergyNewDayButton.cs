using ADSContent.Popups;
using SettingsContent.SoundContent;
using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class KeepEnergyNewDayButton : AbstractButton
    {
        [SerializeField] private AbstractScreen _screen;
        [SerializeField] private AdPopupActivator _adPopupActivator;

        int _index = 0;

        public override void OnClick()
        {
            if (_index <= 0)
            {
                _index++;
                _adPopupActivator.ShowPacks();
            }
            
            SoundPlayer.Instance.PlayButtonClick();
            _screen.CloseScreen();
        }
    }
}