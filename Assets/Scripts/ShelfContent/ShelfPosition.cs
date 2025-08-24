using UnityEngine;

namespace ShelfContent
{
    public class ShelfPosition : MonoBehaviour
    {
        [SerializeField] private ShelfPositionViewer _shelfPositionViewer;
        
        public void Init(Sprite sprite, int value)
        {
            _shelfPositionViewer.Init(sprite, value);
        }

        public void SetActiveValue(int value)
        {
            _shelfPositionViewer.SetActiveValue(value);
        }

        public void Clear()
        {
            _shelfPositionViewer.Clear();
        }
    }
}