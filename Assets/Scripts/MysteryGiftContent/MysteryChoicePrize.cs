using System.Collections.Generic;
using PlayerContent.LevelContent;
using UnityEngine;

namespace MysteryGiftContent
{
    public class MysteryChoicePrize : MonoBehaviour
    {
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private List<MysteryPrize> prizes = new List<MysteryPrize>();



    }
    
    [System.Serializable]
    public class MysteryPrize
    {
        public string name;
        public int level;
    }
}