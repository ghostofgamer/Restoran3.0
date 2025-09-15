using Enums;
using UnityEngine;

namespace CustomizationContent
{
    public class Customization : MonoBehaviour
    {
        [SerializeField]private StyleCustomization _styleCustomization;
        
        public void ChangeStyle(StyleType styleType, int index)
        {
            switch (styleType)
            {
                case StyleType.OutsideWall:
                    _styleCustomization.ChangeOutsideWallTexture(index);
                    break;
                
                case StyleType.InsideWall:
                    _styleCustomization.ChangeInsideWallTexture(index);
                    break;
                
                case StyleType.Floor:
                    _styleCustomization.ChangeFloorTexture(index);
                    break;
                
                case StyleType.Kitchen:
                    _styleCustomization.ChangeKitchenTexture(index);
                    break;
                
                case StyleType.Visor:
                    _styleCustomization.ChangeVisorTexture(index);
                    break;
            }
        }
    }
}