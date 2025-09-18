using SaveSystemContent;
using UnityEngine;

namespace CustomizationContent.SavesCustomization
{
    public class SaveStyleCustomization : MonoBehaviour
    {
        private const string SaveFile = "style_applied";
        [SerializeField] private StyleCustomization _styleCustomization;
        private StyleAppliedData _appliedData = new StyleAppliedData();
        
        public void Save()
        {
            SaveDataGame.SaveJson(SaveFile, _appliedData);
        }

        public void Load()
        {
            var data = LoadDataGame.LoadJson<StyleAppliedData>(SaveFile);

            if (data == null) // первый запуск
            {
                _appliedData = new StyleAppliedData(); // все 0
                Save(); // сразу создаём файл
            }
            else
            {
                _appliedData = data;
            }

            _styleCustomization.ChangeFloorTexture(_appliedData.FloorIndex);
            _styleCustomization.ChangeOutsideWallTexture(_appliedData.OutsideWallIndex);
            _styleCustomization.ChangeInsideWallTexture(_appliedData.InsideWallIndex);
            _styleCustomization.ChangeKitchenTexture(_appliedData.KitchenIndex);
            _styleCustomization.ChangeVisorTexture(_appliedData.VisorIndex);
        }

        public void SetFloor(int index)
        {
            _appliedData.FloorIndex = index;
            Save();
        }

        public void SetOutsideWall(int index)
        {
            _appliedData.OutsideWallIndex = index;
            Save();
        }

        public void SetInsideWall(int index)
        {
            _appliedData.InsideWallIndex = index;
            Save();
        }

        public void SetKitchen(int index)
        {
            _appliedData.KitchenIndex = index;
            Save();
        }

        public void SetVisor(int index)
        {
            _appliedData.VisorIndex = index;
            Save();
        }
    }

    [System.Serializable]
    public class StyleAppliedData
    {   
        public int FloorIndex;
        public int OutsideWallIndex;
        public int InsideWallIndex;
        public int KitchenIndex;
        public int VisorIndex;
    }
}