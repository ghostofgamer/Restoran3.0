using System;
using Enums;
using UnityEngine;

namespace QuestsContent
{
    [Serializable]
    public class PrizeTask
    {
        [SerializeField]private Sprite _icon;
        [SerializeField] private TaskPrizeType _taskPrizeType;
        [SerializeField] private int _amount;
        [SerializeField] private ItemType _itemType;
        
        public TaskPrizeType TaskPrizeType => _taskPrizeType;
        public int Amount => _amount;
        public Sprite Icon => _icon;

        public ItemType ItemType => _itemType;
    }
}