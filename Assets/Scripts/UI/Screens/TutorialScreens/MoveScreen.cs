using TMPro;
using UnityEngine;

namespace UI.Screens.TutorialScreens
{
    public class MoveScreen : AbstractScreen
    {
        [SerializeField] private TMP_Text _title;

        private void Start()
        {
            _title.text = "Use <color=yellow>left thumb</color> to move";
        }
    }
}