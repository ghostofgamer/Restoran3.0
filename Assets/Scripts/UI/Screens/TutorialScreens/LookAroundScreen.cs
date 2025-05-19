using TMPro;
using UnityEngine;

namespace UI.Screens.TutorialScreens
{
    public class LookAroundScreen : AbstractScreen
    {
        [SerializeField] private TMP_Text _title;

        private void Start()
        {
            _title.text = "Use <color=yellow>right thumb</color> to look around";
        }
    }
}