using UnityEngine;

namespace TaskContent.FortuneTaskContent
{
    public class GlowRotation : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(Vector3.forward, 5 * Time.deltaTime);
        }
    }
}