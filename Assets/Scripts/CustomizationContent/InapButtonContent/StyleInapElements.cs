using UnityEngine;

namespace CustomizationContent.InapButtonContent
{
    public class StyleInapElements : MonoBehaviour
    {
        [SerializeField]private string _key;
        
        private void OnEnable()
        {
            int value = PlayerPrefs.GetInt(_key, 0);
            gameObject.SetActive(value == 0);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}