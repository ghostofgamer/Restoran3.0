using System;
using InteractableContent;
using TMPro;
using UnityEngine;

namespace ItemContent
{
    public class ItemContainerViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private ItemContainer _itemContainer;
        [SerializeField] private GameObject _containerObject;
        [SerializeField] private CanvasRotator _canvasRotator;

        private bool _assemblyMode;
        private Quaternion _defaultRotation;

        private void Awake()
        {
            _defaultRotation = _containerObject.transform.rotation;
        }

        private void OnEnable()
        {
            _itemContainer.ValueChanged += ShowValue;
        }

        private void OnDisable()
        {
            _itemContainer.ValueChanged -= ShowValue;
        }

        public void SetActive(bool value)
        {
            Debug.Log("выаываапываывапывапва " + value);

            _containerObject.SetActive(value);
            _canvasRotator.enabled = !_assemblyMode;

            if (_assemblyMode)
            {
                _containerObject.transform.rotation = _defaultRotation;
                // _containerObject.transform.position = _defaultPosition.position;
                // _containerObject.transform.localRotation = _defaultPosition.localRotation;
                // Debug.Log("_defaultRotation " + _defaultPosition.localRotation);
                Debug.Log("_containerObject.transform.rotation " + _containerObject.transform.rotation);
            }
        }

        public void SetAssemblyMode(bool value)
        {
            _assemblyMode = value;
        }

        private void ShowValue(int currentValue, int maxValue)
        {
            _valueText.text = $"{currentValue}/{maxValue}";
        }
    }
}