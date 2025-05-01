using Enums;

namespace DeliveryContent
{
    [System.Serializable]
    public class ItemDeliveryInfo
    {
        public ItemType ItemType { get; set; }
        public int Amount { get; set; }
    }
}