using Enums;
using UnityEngine;

namespace AssemblyBurgerContent
{
    public class Souce : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType;

        public ItemType ItemType => _itemType;
    }
}
