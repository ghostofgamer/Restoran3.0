using UnityEngine;

namespace NotificationContent
{
    public abstract class AbstractNotification<T> : MonoBehaviour
    {
        public abstract void Init(T data);
    }
}