using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyBurgerContent;
using Enums;
using TMPro;
using UI.Buttons;
using UnityEngine;

public class CreateTestBurger : AbstractButton
{
    [SerializeField] private ItemType _itemType;
    [SerializeField] private List<ItemType> _itemTypes;
    [SerializeField] private AssemblyBurger _assemblyBurger;
    [SerializeField] private TMP_Text _nameItemType;

    private void Start()
    {
        if (_itemTypes.Count > 0)
        {
            _itemType = _itemTypes[0];
            _nameItemType.text = _itemType.ToString();
        }
    }

    public void ChangeItemType(ItemType itemType)
    {
        _itemType = itemType;
        _nameItemType.text = _itemType.ToString();
    }
    
    public override void OnClick()
    {
        _assemblyBurger.CreateFreeBurgerTestCheat(_itemType);
    }
    
    public void CycleItemTypeUp()
    {
        if (_itemTypes.Count == 0) return;

        int currentIndex = _itemTypes.IndexOf(_itemType);
        int nextIndex = (currentIndex + 1) % _itemTypes.Count;
        _itemType = _itemTypes[nextIndex];
        _nameItemType.text = _itemType.ToString();
    }

    public void CycleItemTypeDown()
    {
        if (_itemTypes.Count == 0) return;

        int currentIndex = _itemTypes.IndexOf(_itemType);
        int prevIndex = (currentIndex - 1 + _itemTypes.Count) % _itemTypes.Count;
        _itemType = _itemTypes[prevIndex];
        _nameItemType.text = _itemType.ToString();
    }

    public void TestCodeium()
    {
        int value = 0;
    }
}