using System.Collections.Generic;
using SaveSystemContent;
using UnityEngine;

namespace CustomizationContent.SavesCustomization
{
    public class SaveDecorCustomization : MonoBehaviour
    {
        private const string SaveFile = "decor_applied";
        
        [SerializeField] private DecorCustomization _decorCustomization;
        
        private DecorAppliedData _appliedData = new DecorAppliedData();

        public void Save()
        {
            _appliedData.Plants = SaveList(_decorCustomization.Plants);
            _appliedData.Paintings = SaveList(_decorCustomization.Paintings);
            _appliedData.Stickers = SaveList(_decorCustomization.Stickers);
            _appliedData.Shelves = SaveList(_decorCustomization.Shelves);
            _appliedData.Others = SaveList(_decorCustomization.Others);

            SaveDataGame.SaveJson(SaveFile, _appliedData);
        }

        private List<bool> SaveList(GameObject[] objects)
        {
            var list = new List<bool>();
            foreach (var obj in objects)
                list.Add(obj.activeSelf);
            return list;
        }

        public void Load()
        {
            var data = LoadDataGame.LoadJson<DecorAppliedData>(SaveFile);

            if (data == null)
            {
                // создаём списки нужной длины, все false
                _appliedData = new DecorAppliedData();
            }
            else
            {
                _appliedData = data;
            }
            
            EnsureListSize(_appliedData.Plants, _decorCustomization.Plants.Length);
            EnsureListSize(_appliedData.Paintings, _decorCustomization.Paintings.Length);
            EnsureListSize(_appliedData.Stickers, _decorCustomization.Stickers.Length);
            EnsureListSize(_appliedData.Shelves, _decorCustomization.Shelves.Length);
            EnsureListSize(_appliedData.Others, _decorCustomization.Others.Length);
            
            ApplyList(_decorCustomization.Plants, _appliedData.Plants);
            ApplyList(_decorCustomization.Paintings, _appliedData.Paintings);
            ApplyList(_decorCustomization.Stickers, _appliedData.Stickers);
            ApplyList(_decorCustomization.Shelves, _appliedData.Shelves);
            ApplyList(_decorCustomization.Others, _appliedData.Others);
        }
        
        private void EnsureListSize(List<bool> list, int size)
        {
            if (list.Count > size)
            {
                // обрезаем лишние элементы
                list.RemoveRange(size, list.Count - size);
            }
            else
            {
                // добавляем новые элементы, если нужно
                while (list.Count < size)
                    list.Add(false);
            }
        }

        private void ApplyList(GameObject[] objects, List<bool> states)
        {
            int count = Mathf.Min(objects.Length, states.Count);
            for (int i = 0; i < count; i++)
                objects[i].SetActive(states[i]);
        }
    }
    
    [System.Serializable]
    public class DecorAppliedData
    {
        public List<bool> Plants = new List<bool>();
        public List<bool> Paintings = new List<bool>();
        public List<bool> Stickers = new List<bool>();
        public List<bool> Shelves = new List<bool>();
        public List<bool> Others = new List<bool>();
    }
}