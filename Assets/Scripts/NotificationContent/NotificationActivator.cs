using UnityEngine;

namespace NotificationContent
{
    public class NotificationActivator : MonoBehaviour
    {
        public static NotificationActivator Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ActivateNotification()
        {
            
        }
    }
}