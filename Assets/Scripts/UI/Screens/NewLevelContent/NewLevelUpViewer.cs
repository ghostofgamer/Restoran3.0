using System.Linq;
using I2.Loc;
using PlayerContent.LevelContent;
using SoContent;
using TMPro;
using UnityEngine;

namespace UI.Screens.NewLevelContent
{
    public class NewLevelUpViewer : MonoBehaviour
    {
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private TMP_Text _levelStarText;
        [SerializeField] private TMP_Text _leverRestText;

        [SerializeField] private TMP_Text _newProductText;
        [SerializeField] private TMP_Text _newRecipesText;
        [SerializeField] private TMP_Text _newMAchineText;
        [SerializeField] private RewardsLevelingUpConfig _rewardsLevelingUpConfig;

        public void Init()
        {
            _levelStarText.text = _playerLevel.CurrentLevel.ToString();
            _leverRestText.text = $"{LocalizationManager.GetTermTranslation("Level")}-{_playerLevel.CurrentLevel}";

            RewardLeveling rewardLeveling = _rewardsLevelingUpConfig.GetLevelData(_playerLevel.CurrentLevel);

            string productsList = string.Join(", ",
                rewardLeveling.products.Select(product => LocalizationManager.GetTermTranslation(product.ToString())));
            
            string recipesList = string.Join(", ",
                rewardLeveling.recipes.Select(product => LocalizationManager.GetTermTranslation(product.ToString())));
            
            string machineList = string.Join(", ",
                rewardLeveling.equipment.Select(product => LocalizationManager.GetTermTranslation(product.ToString())));

            if (rewardLeveling != null)
            {
                _newProductText.text = $"{LocalizationManager.GetTermTranslation("NewProduct")} {productsList}";
                _newRecipesText.text = $"{LocalizationManager.GetTermTranslation("NewRecipes")} {recipesList}";
                _newMAchineText.text = $"{LocalizationManager.GetTermTranslation("NewEquipment")} {machineList}";
            }
        }
    }
}