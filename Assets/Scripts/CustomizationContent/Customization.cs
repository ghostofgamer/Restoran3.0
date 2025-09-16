using Enums;
using UnityEngine;

namespace CustomizationContent
{
    public class Customization : MonoBehaviour
    {
        [SerializeField] private StyleCustomization _styleCustomization;
        [SerializeField] private FurnitureCustomization _furnitureCustomization;
        [SerializeField]private DecorCustomization _decorCustomization;

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

                case StyleType.SofaFurniture:
                    _furnitureCustomization.ChangeSofaMaterial(index);
                    break;

                case StyleType.TableFurniture:
                    _furnitureCustomization.ChangeTableMaterial(index);
                    break;
                
                case StyleType.ChairFurniture:
                    _furnitureCustomization.ChangeChairMaterial(index);
                    break;
                
                case StyleType.Plants:
                    _decorCustomization.ChangeActivityPlants(index);
                    break;
                
                case StyleType.Paintings:
                    _decorCustomization.ChangeActivityPaintings(index);
                    break;
                
                case StyleType.Stickers:
                    _decorCustomization.ChangeActivityStickers(index);
                    break;
                
                case StyleType.Shelves:
                    _decorCustomization.ChangeActivityShelves(index);
                    break;
                
                case StyleType.Others:
                    _decorCustomization.ChangeActivityOthers(index);
                    break;
            }
        }
    }
}