using System;
using UnityEngine;

namespace ADSContent
{
    public class InterstitialActivator : MonoBehaviour
    {
        [SerializeField] private ADS _ads;
        
        private static InterstitialActivator _instance;
        private TimeSpan adCooldown = TimeSpan.FromMinutes(2);
        private const string LastADKey = "LastAdInterShow";

        public static InterstitialActivator Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("AdManager");
                    _instance = obj.AddComponent<InterstitialActivator>();
                    DontDestroyOnLoad(obj);
                }
                
                return _instance;
            }
        }

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void ShowAd()
        {
            if (CanShowAd())
            {
                _ads.ShowInterstitial();
                Debug.Log("$$$Showing Ad");
                PlayerPrefs.SetString(LastADKey, DateTime.UtcNow.Ticks.ToString());
                PlayerPrefs.Save();
            }
            else
            {
                Debug.Log("$$$Ad cooldown active");
            }
        }

        private bool CanShowAd()
        {
            // Если никогда не показывали - разрешаем
            if (!PlayerPrefs.HasKey(LastADKey)) return true;
        
            long storedTicks = long.Parse(PlayerPrefs.GetString(LastADKey));
            var lastTime = new DateTime(storedTicks, DateTimeKind.Utc);
            var currentTime = DateTime.UtcNow;
        Debug.Log("currentTime - lastTime" + (currentTime - lastTime)); 
            return (currentTime - lastTime) >= adCooldown;
        }
    }
}