using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Enums;
using ItemContent;
using Unity.VisualScripting;
using UnityEngine;

namespace SaveContent
{
    public class BoxSaver : MonoBehaviour
    {
        [SerializeField] private BoxesCounter _boxesCounter;

        private List<ItemType> _itemBaskets = new List<ItemType>();

        private void Start()
        {
            LoadData();
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void SaveData()
        {
            // Преобразуем данные коробок в формат для сохранения
            List<BoxData> boxesToSave = _boxesCounter.ItemBaskets
                .Select(item => new BoxData((int)item.ItemType, item.transform.position, item.GetActiveValueItems(),
                    item.IsAdditionalItemsBasket, item.GetActiveValueArrayItems()))
                .Concat(_boxesCounter.ItemDrinkPackages
                    .Select(item => new BoxData((int)item.ItemType, item.transform.position, item.CurrentFullness,
                        false, null)))
                .ToList();

            // Сохраняем данные в JSON файл
            string jsonData = JsonUtility.ToJson(new BoxDataWrapper(boxesToSave));
            string path = Application.persistentDataPath + "/boxData.json";
            File.WriteAllText(path, jsonData);
        }

        public List<BoxData> LoadData()
        {
            // Загружаем данные из JSON файла
            string path = Application.persistentDataPath + "/boxData.json";
            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);
                BoxDataWrapper wrapper = JsonUtility.FromJson<BoxDataWrapper>(jsonData);
                return wrapper.boxes;
            }

            return new List<BoxData>();
        }

        [ContextMenu("ClearSavedData")]
        public void ClearSavedData()
        {
            _boxesCounter.Clear();

            string path = Application.persistentDataPath + "/boxData.json";
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Saved data cleared.");
            }
            else
            {
                Debug.Log("No saved data found.");
            }
        }


        /*private void SaveDate()
        {
            int[] combinedIndices = _boxesCounter.ItemBaskets.Select(item => (int)item.ItemType).ToArray();

            string combinedIndicesString = string.Join(",", combinedIndices);

            /*int[] combinedIndices = itemBasketList.Select(item => (int)item.ItemType)
                .Concat(itemDrinkList.Select(item => (int)item.ItemType))
                .ToArray();#1#

            // string combinedIndicesString = string.Join(",", combinedIndices);

            PlayerPrefs.SetString("combinedBoxesIndices", combinedIndicesString);
            PlayerPrefs.Save();
        }

        public List<ItemType> LoadData()
        {
            string combinedIndicesString = PlayerPrefs.GetString("combinedBoxesIndices", "");

            if (!string.IsNullOrEmpty(combinedIndicesString))
            {
                string[] indicesArray = combinedIndicesString.Split(',');
                int[] indices = Array.ConvertAll(indicesArray, int.Parse);

                foreach (var index in indices)
                    _itemBaskets.Add((ItemType)index);

                return _itemBaskets;
            }

            return new List<ItemType>();
        }*/
    }

    [System.Serializable]
    public struct BoxData
    {
        public int itemType;
        public Vector3 position;
        public int amount;
        public bool additional;
        public int[] additionalAmountItems;

        public BoxData(int type, Vector3 pos, int amount, bool additional, int[] additionalAmount)
        {
            itemType = type;
            position = pos;
            this.amount = amount;
            this.additional = additional;
            additionalAmountItems = additionalAmount;
        }
    }

    [System.Serializable]
    public class BoxDataWrapper
    {
        public List<BoxData> boxes;

        public BoxDataWrapper(List<BoxData> boxes)
        {
            this.boxes = boxes;
        }
    }
}