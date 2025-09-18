using SaveSystemContent;
using UnityEngine;

namespace CustomizationContent.SavesCustomization
{
    public class SaveFurnitureCustomization : MonoBehaviour
    {
        private const string SaveFile = "furniture_applied";
        
        [SerializeField] private FurnitureCustomization _furnitureCustomization;
        
        private FurnitureAppliedData _appliedData = new FurnitureAppliedData();

        private void Save()
        {
            SaveDataGame.SaveJson(SaveFile, _appliedData);
        }
        
        public void Load()
        {
            var data = LoadDataGame.LoadJson<FurnitureAppliedData>(SaveFile);

            if (data == null) // первый запуск
            {
                _appliedData = new FurnitureAppliedData(); // все 0
                Save(); // создаём файл
            }
            else
            {
                _appliedData = data;
            }

            // Применяем сохранённые цвета через FurnitureCustomization
            _furnitureCustomization.ChangeSofaMaterial(_appliedData.SofaIndex);
            _furnitureCustomization.ChangeTableMaterial(_appliedData.TableIndex);
            _furnitureCustomization.ChangeChairMaterial(_appliedData.ChairIndex);
        }
        
        public void SetSofa(int index)
        {
            _appliedData.SofaIndex = index;
            Save();
        }

        public void SetTable(int index)
        {
            _appliedData.TableIndex = index;
            Save();
        }

        public void SetChair(int index)
        {
            _appliedData.ChairIndex = index;
            Save();
        }
    }
    
    [System.Serializable]
    public class FurnitureAppliedData
    {
        public int SofaIndex = 0;
        public int TableIndex = 0;
        public int ChairIndex = 0;
    }
}