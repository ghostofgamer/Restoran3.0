using System;
using UnityEngine;
using WalletContent;

namespace SoContent.ShopStyleSOContent
{
    [CreateAssetMenu(fileName = "NewStyleConfig", menuName = "Configs/StyleConfig")]
    public class StyleSoConfig : ScriptableObject
    {
        [SerializeField] private StyleSoConfigElement[] _styleSoConfigElements;
        
        public StyleSoConfigElement[] StyleSoConfigElements => _styleSoConfigElements;
    }

    [Serializable]
    public class StyleSoConfigElement
    {
        [SerializeField] private DollarValue _dollarValue;
        [SerializeField] private int _openLevel;
        [SerializeField] private bool _isRewardStyle;
        [SerializeField] private bool _isOpenStart;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private bool _requiresZoneUnlock;
        [SerializeField] private int _zoneIndex;
     
        public DollarValue DollarValue => _dollarValue;
        public int OpenLevel => _openLevel;
        public bool IsRewardStyle => _isRewardStyle;
        public bool IsOpenStart => _isOpenStart;
        public Sprite Sprite => _sprite;
        public bool RequiresZoneUnlock => _requiresZoneUnlock;
        public int ZoneIndex => _zoneIndex;
    }
}