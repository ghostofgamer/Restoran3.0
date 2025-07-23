using Enums;
using UnityEngine;

namespace QuestsContent.TaskLinears
{
    [CreateAssetMenu(fileName = "PurchaseItemTask", menuName = "Configs/PurchaseItemTaskConfig", order = 1)]
    public class PurchaseItemTask : Task
    {
        [SerializeField] private ItemType _itemType;

        public override bool CheckCompletion()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateTask()
        {

        }

        protected override void Initialization()
        {
            
        }

        protected override void SubscribeToEvents()
        {

        }

        protected override void UnsubscribeFromEvents()
        {
        }
    }
}