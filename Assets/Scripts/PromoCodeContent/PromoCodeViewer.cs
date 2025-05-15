using System;
using Enums;
using TMPro;
using UI.Screens;
using UnityEngine;

namespace PromoCodeContent
{
    public class PromoCodeViewer : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _promoCodeInputField;
        [SerializeField] private TMP_Text _messageFailedText;
        [SerializeField] private GameObject _failTextBackground;
        [SerializeField] private PromoCodeScreen _promoCodeScreen;

        public void AcceptPromoCode()
        {
            string enteredCode = _promoCodeInputField.text.Trim().ToUpper();
            PromoCodesType currentPromoCode = PromoCodesType.BurgerBoss;

            string activePromoCode = currentPromoCode.ToString().ToUpper();

            if (enteredCode == activePromoCode)
            {
                if (PlayerPrefs.GetInt("AcceptedCode" + currentPromoCode, 0) == 1)
                {
                    Debug.Log("Этот промо-код уже был куплен.");
                    _messageFailedText.text = "Этот промо-код уже был куплен.";
                    _failTextBackground.SetActive(true);
                }
                else
                {
                    PlayerPrefs.SetInt("AcceptedCode" + currentPromoCode, 1);
                    PlayerPrefs.Save();
                    _promoCodeScreen.CloseScreen();
                }
            }
            else
            {
                Debug.Log("Неверный промо-код.");
                _messageFailedText.text = "Неверный промо-код.";
                _failTextBackground.SetActive(true);
            }
            
            _promoCodeInputField.text = "";
        }
    }
}