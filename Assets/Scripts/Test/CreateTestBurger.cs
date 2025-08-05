using AssemblyBurgerContent;
using Enums;
using UI.Buttons;
using UnityEngine;

public class CreateTestBurger : AbstractButton
{
    [SerializeField] private ItemType _itemType;
    [SerializeField] private AssemblyBurger _assemblyBurger;

    public override void OnClick()
    {
        _assemblyBurger.CreateFreeBurgerTestCheat(_itemType);
    }
}