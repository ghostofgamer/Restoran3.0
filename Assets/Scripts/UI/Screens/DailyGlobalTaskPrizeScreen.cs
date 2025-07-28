using System.Collections.Generic;
using MysteryGiftContent;
using UnityEngine;

namespace UI.Screens
{
    public class DailyGlobalTaskPrizeScreen : AbstractScreen
    {
        [SerializeField] private DailyGlobalPrize[] _prizes;
        
        public List<MysteryPrize> RandomPrizes { get; private set; } = new List<MysteryPrize>();

        public void Init(List<MysteryPrize> prizes)
        {
            RandomPrizes = prizes;
            
            if (prizes.Count != _prizes.Length)
                return;
            
            for (int i = 0; i < prizes.Count; i++)
                _prizes[i].SetValue(prizes[i].SpriteIcon,prizes[i].Value);
        }
    }
}