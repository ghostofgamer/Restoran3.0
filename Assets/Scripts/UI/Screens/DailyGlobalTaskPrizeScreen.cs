using System.Collections.Generic;
using MysteryGiftContent;
using UnityEngine;

namespace UI.Screens
{
    public class DailyGlobalTaskPrizeScreen : AbstractScreen
    {
        [SerializeField] private DailyGlobalPrize[] _prizes;
        [SerializeField] private GameObject[] _effects;
        
        public List<MysteryPrize> RandomPrizes { get; private set; } = new List<MysteryPrize>();

        public void Init(List<MysteryPrize> prizes)
        {
            RandomPrizes = prizes;
            // SetActiveEffects(true);
            
            if (prizes.Count != _prizes.Length)
            {
                Debug.Log("Не совпадает колличесвто!");
                return;
            }
            
            for (int i = 0; i < prizes.Count; i++)
                _prizes[i].SetValue(prizes[i].SpriteIcon,prizes[i].Value);
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
            // SetActiveEffects(false);
        }

        private void SetActiveEffects(bool value)
        {
            foreach (var effect in _effects)
                effect.SetActive(value);
        }
    }
}