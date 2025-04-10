using UI.Screens;
using UnityEngine;

namespace UI.Buttons.SettingsScreenButton
{
    public class OurGamingSiteButton : AbstractButton
    {
        [SerializeField] private SettingsScreen _settingsScreen;

        public override void OnClick() => _settingsScreen.OpenLifeFrameSite();
    }
}