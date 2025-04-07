using UnityEngine;

namespace UI.Screens
{
    public abstract class AbstractScreen : MonoBehaviour
    {
        public virtual void OpenScreen()
        {
            gameObject.SetActive(true);
        }

        public virtual void CloseScreen()
        {
            gameObject.SetActive(false);
        }
    }
}