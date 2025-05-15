using PromoCodeContent;
using UnityEngine;

namespace UI.Buttons
{
    public class AcceptPromoCodeButton : AbstractButton
    {
        [SerializeField] private PromoCodeViewer _promoCodeViewer;
        
        public override void OnClick()
        {
            _promoCodeViewer.AcceptPromoCode();
        }
    }
}