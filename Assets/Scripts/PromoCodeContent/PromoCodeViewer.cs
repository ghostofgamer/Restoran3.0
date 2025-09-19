using System;
using System.Linq;
using Enums;
using I2.Loc;
using TMPro;
using UI.Screens;
using UnityEngine;

namespace PromoCodeContent
{
    public class PromoCodeViewer : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _promoCodeInputField;
        [SerializeField] private TMP_Text _messageFailedText;
        [SerializeField] private TMP_Text _messageWellDoneText;
        [SerializeField] private GameObject _failTextBackground;
        [SerializeField] private GameObject _WellDoneTextBackground;
        [SerializeField] private PromoCodeScreen _promoCodeScreen;
        [SerializeField] private PromoCodeActivator _promoCodeActivator;
        
        public void AcceptPromoCode()
        {
            string enteredCode = _promoCodeInputField.text.Trim().ToUpper();

            // ищем код среди активных призов
            PromoCodePrize prize = _promoCodeActivator
                .PromoCodePrizes
                .FirstOrDefault(p => p.PromoCodeType.ToString().Equals(enteredCode, StringComparison.OrdinalIgnoreCase));

            if (prize != null) // нашли код
            {
                PromoCodesType promoCodeType = prize.PromoCodeType;

                if (PlayerPrefs.GetInt("AcceptedCode" + promoCodeType, 0) == 1)
                {
                    Debug.Log("Этот промо-код уже был использован.");
                    // _messageFailedText.text = LocalizationManager.GetTermTranslation("This promo code has already been used.");
                    _messageFailedText.text = LocalizationManager.GetTermTranslation("This promo code has already been purchased.");
                    _failTextBackground.SetActive(true);
                }
                else
                {
                    _promoCodeActivator.ActivatePrizePromo(promoCodeType);
                    PlayerPrefs.SetInt("AcceptedCode" + promoCodeType, 1);
                    PlayerPrefs.Save();
                    _messageWellDoneText.text = LocalizationManager.GetTermTranslation("Right! Get prizes in the delivery area!");
                    _WellDoneTextBackground.SetActive(true);
                }
            }
            else
            {
                Debug.Log("Неверный промо-код.");
                _messageFailedText.text = LocalizationManager.GetTermTranslation("Invalid promo code.");
                _failTextBackground.SetActive(true);
            }

            _promoCodeInputField.text = "";
        }
    }
}