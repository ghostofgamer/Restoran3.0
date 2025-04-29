using UnityEngine;
using UnityEngine.UI;

namespace OrdersContent
{
    public class ElementSelector : MonoBehaviour
    {
        [SerializeField] private HorizontalLayoutGroup _layoutGroup;
        [SerializeField] private int _indexElement = 2;
        [SerializeField] private float _spacingBeforeHighlight = 30f;
        [SerializeField] private float _defaultSpacing = 10f; 
        [SerializeField] private float _increasedSpacing = 50f;
        [SerializeField] private GameObject _spacer;
        
        void Start()
        {
            if (_spacer != null)
                _spacer.SetActive(false);
        }
        
        public void UpdateSpacing(int startIndex)
        {
          
            if (_layoutGroup == null)
            {
                Debug.LogError("HorizontalLayoutGroup is not assigned.");
                return;
            }

            if (_spacer == null)
            {
                Debug.LogError("Spacer is not assigned.");
                return;
            }
            
            _layoutGroup.spacing = _defaultSpacing;
            
            if (startIndex >= 0 && startIndex < _layoutGroup.transform.childCount)
            {
                _spacer.SetActive(true);
                _spacer.GetComponent<LayoutElement>().preferredWidth = _increasedSpacing;
                _spacer.transform.SetParent(_layoutGroup.transform);
                _spacer.transform.SetSiblingIndex(startIndex + 2);
            }
            else
            {
                _spacer.SetActive(false);
            }
        }

        public void ReturnDefaultSpacing()
        {
            if (_layoutGroup == null)
            {
                Debug.LogError("HorizontalLayoutGroup is not assigned.");
                return;
            }

            if (_spacer != null)
            {
                _spacer.SetActive(false);
                _spacer.GetComponent<LayoutElement>().preferredWidth = 0; // Reset the spacer's width
                _spacer.transform.SetParent(null); // Remove the spacer from the layout group
            }

            _layoutGroup.spacing = _defaultSpacing;
        }
    }
}