using System.Collections.Generic;
using Enums;

namespace IAP
{
    public static class IapIds
    {
        private static readonly Dictionary<IapProductType, string> _ids = new()
        {
            { IapProductType.Money100, "com.serbull.iaptutorial.money100" },
            { IapProductType.RemoveAds, "com.serbull.iaptutorial.removeads" },
            { IapProductType.Money500, "com.serbull.iaptutorial.money500" },
            { IapProductType.Money1100, "com.serbull.iaptutorial.money1100" },
            { IapProductType.Money2750, "com.serbull.iaptutorial.money2750" },
            { IapProductType.Money8000, "com.serbull.iaptutorial.money8000" },
            { IapProductType.Money20000, "com.serbull.iaptutorial.money20000" },
            { IapProductType.StarterPack, "com.serbull.iaptutorial.starterpack" },
            { IapProductType.Energy30, "com.serbull.iaptutorial.energy30" },
            { IapProductType.Energy150, "com.serbull.iaptutorial.energy150" },
            { IapProductType.Energy450, "com.serbull.iaptutorial.energy450" },
            { IapProductType.Energy1850, "com.serbull.iaptutorial.energy1850" },
            { IapProductType.Energy5000, "com.serbull.iaptutorial.energy5000" },
            { IapProductType.StoragePack, "com.serbull.iaptutorial.storagepack" },
            { IapProductType.StylePack, "com.serbull.iaptutorial.stylePack" }
        };

        public static string GetId(IapProductType type) => _ids[type];
    }
}