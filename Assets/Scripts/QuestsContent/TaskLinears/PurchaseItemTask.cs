using Enums;
using UnityEngine;

namespace QuestsContent.TaskLinears
{
    [CreateAssetMenu(fileName = "PurchaseItemTask", menuName = "QuestConfigs/PurchaseItemTaskConfig", order = 1)]
    public class PurchaseItemTask : Task
    {
        [SerializeField] private ItemType _itemType;

        public override bool CheckCompletion()
        {
            throw new System.NotImplementedException();
        }

        protected override void Initialization()
        {
            
        }

        protected override void SubscribeToEvents()
        {

        }

        public override void UnsubscribeFromEvents()
        {
        }

        public override void LocalizationChanged()
        {
            
        }
    }
}