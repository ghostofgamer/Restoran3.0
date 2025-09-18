using System;
using CustomizationContent.SavesCustomization;
using UnityEngine;

namespace CustomizationContent
{
    public class FurnitureCustomization : MonoBehaviour
    {
        [SerializeField]private SaveFurnitureCustomization _saveFurnitureCustomization;
        [SerializeField] private Material[] _placeMaterials;
        [SerializeField] private Material[] _tableMaterials;
        [SerializeField] private Material[] _chairMaterials;
        [SerializeField] private PlaceColorData[] _placeColorDataSofas;
        [SerializeField] private PlaceColorData[] _placeColorDataTables;
        [SerializeField] private PlaceColorData[] _placeColorDataChairs;

        private void ChangeMaterialsColor(Material[] materials, PlaceColorData[] colorDataArray, int colorIndex)
        {
            if (materials == null || materials.Length == 0)
            {
                Debug.LogWarning("Материалы не заданы!");
                return;
            }

            if (colorDataArray == null || colorDataArray.Length == 0)
            {
                Debug.LogWarning("Данные цветов не заданы!");
                return;
            }

            if (colorIndex < 0 || colorIndex >= colorDataArray.Length)
            {
                Debug.LogWarning("Индекс цвета вне диапазона!");
                return;
            }

            PlaceColorData colorData = colorDataArray[colorIndex];

            if (colorData.Colors == null || colorData.Colors.Length == 0)
            {
                Debug.LogWarning("Массив цветов пуст!");
                return;
            }

            int colorCount = Mathf.Min(materials.Length, colorData.Colors.Length);

            for (int i = 0; i < colorCount; i++)
                materials[i].color = colorData.Colors[i];
        }

        public void ChangeSofaMaterial(int index)
        {
            ChangeMaterialsColor(_placeMaterials, _placeColorDataSofas, index);
            _saveFurnitureCustomization.SetSofa(index);
        }

        public void ChangeTableMaterial(int index)
        {
            ChangeMaterialsColor(_tableMaterials, _placeColorDataTables, index);
            _saveFurnitureCustomization.SetTable(index);
        }

        public void ChangeChairMaterial(int index)
        {
            ChangeMaterialsColor(_chairMaterials, _placeColorDataChairs, index);
            _saveFurnitureCustomization.SetChair(index);
        }
    }
}

[Serializable]
public class PlaceColorData
{
    public Color[] Colors;
}