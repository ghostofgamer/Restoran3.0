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
        
        public TaskPrizeType TaskPrizeType => _taskPrizeType;
        public int Amount => _amount;
        public Sprite Icon => _icon;
    }
}