using UnityEngine;

namespace PromoCodeContent
{
    public class WarningTextBanner : MonoBehaviour
    {
        private void OnDisable()
        {
            Debug.Log("ыываываыаваыаыаыаыыааы");
            gameObject.SetActive(false);
        }

        public void SETVALUEGAMEOBJECT()
        {
            gameObject.SetActive(false);
        }
    }
}