using System;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace ItemContent
{
    public class ItemDrinkPackage : MonoBehaviour
    {
        [SerializeField] private Draggable _draggable;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private Image _imageFullness;
        [SerializeField] private GameObject _canvasFullness;

        private int _maxFullnes = 100;

        public int CurrentFullness { get; private set; }

        public ItemType ItemType => _itemType;

        private void OnEnable()
        {
            _draggable.DraggablePicked += () => _canvasFullness.gameObject.SetActive(true);
            _draggable.DraggableThrowed += () => _canvasFullness.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _draggable.DraggablePicked -= () => _canvasFullness.gameObject.SetActive(true);
            _draggable.DraggableThrowed -= () => _canvasFullness.gameObject.SetActive(false);
        }

        private void Start()
        {
            CurrentFullness = _maxFullnes;
            UpdateFullUI();
        }

        public void PourOut(int value)
        {
            CurrentFullness -= value;

            if (CurrentFullness <= 0)
                CurrentFullness = 0;

            UpdateFullUI();
        }

        private void UpdateFullUI()
        {
            _imageFullness.fillAmount = (float)CurrentFullness / _maxFullnes;
        }
    }
}