using TMPro;
using UnityEngine;

namespace UI.Screens.AdsScreens
{
    public class RemoveAdScreen : AbstractScreen
    {
        [SerializeField] private TMP_Text _descriptionText;

        private void Start()
        {
            _descriptionText.text = $"NO ADS\n<color=yellow>FOREVER</color>";
        }
    }
}