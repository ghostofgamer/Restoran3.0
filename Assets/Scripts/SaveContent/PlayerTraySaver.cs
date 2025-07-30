using System;
using Enums;
using PlayerContent;
using UnityEngine;

namespace SaveContent
{
    public class PlayerTraySaver : MonoBehaviour
    {
        [SerializeField] private PlayerTray _playerTray;

        private int _rawCutletTrayValue;
        private int _wellCutletTrayValue;

        private void OnEnable()
        {
            _playerTray.CutletItemsActiveChanged += SaveData;
        }

        private void OnDisable()
        {
            _playerTray.CutletItemsActiveChanged -= SaveData;
        }

        private void Start()
        {
            LoadData();
        }

        private void SaveData(int valueRaw, int valueWell)
        {
            PlayerPrefs.SetInt("RawCutletTrayValue", valueRaw);
            PlayerPrefs.SetInt("WellCutletTrayValue", valueWell);
        }

        public void LoadData()
        {
            int rawValue = PlayerPrefs.GetInt("RawCutletTrayValue", 0);
            int wellValue = PlayerPrefs.GetInt("WellCutletTrayValue", 0);

            if (rawValue > 0 || wellValue > 0)
            {
                if (rawValue > 0)
                    SetValue(ItemType.RawCutlet, rawValue);

                if (wellValue > 0)
                    SetValue(ItemType.Cutlet, wellValue);
            }
            else
            {
                _playerTray.SetActive(false);
                _playerTray.SetCurrentItemType(ItemType.Empty);
                gameObject.SetActive(false);
            }
        }

        private void SetValue(ItemType itemType,int value)
        {
            _playerTray.Put(itemType, value);
            _playerTray.SetCurrentItemType(itemType);
        }
    }
}