using FortuneContent;
using UnityEngine;

namespace UI.Buttons
{
    public class OpenFortuneButton : AbstractButton
    {
        [SerializeField] private Fortune _fortune;
        
        public override void OnClick()
        {
            _fortune.OnShow();
        }
    }
}