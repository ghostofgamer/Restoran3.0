using System;
using UnityEngine;

namespace ADSContent
{
    public class InterstitialActivator : MonoBehaviour
    {
        private static InterstitialActivator _instance;
        private const string LastADKey = "LastAdInterShow";
        private const string FirstLaunchKey = "FirstLaunchTime";
        
        [SerializeField] private ADS _ads;
        [SerializeField] private float _duration; 
        
        private TimeSpan adCooldown;
        private DateTime _sessionStartTime;

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
            adCooldown = TimeSpan.FromMinutes(_duration);
            
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeSession();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void InitializeSession()
        {
            _sessionStartTime = DateTime.UtcNow;

            // Если это первый запуск - сохраняем время первого запуска
            if (!PlayerPrefs.HasKey(FirstLaunchKey))
            {
                PlayerPrefs.SetString(FirstLaunchKey, _sessionStartTime.Ticks.ToString());
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
            DateTime currentTime = DateTime.UtcNow;

            // 1. Проверка времени с первого запуска приложения
            if (PlayerPrefs.HasKey(FirstLaunchKey))
            {
                long firstLaunchTicks = long.Parse(PlayerPrefs.GetString(FirstLaunchKey));
                DateTime firstLaunchTime = new DateTime(firstLaunchTicks, DateTimeKind.Utc);

                if ((currentTime - firstLaunchTime) < adCooldown)
                {
                    Debug.Log("Ad not ready: first launch cooldown");
                    return false;
                }
            }

            // 2. Проверка времени с начала текущей сессии
            if ((currentTime - _sessionStartTime) < adCooldown)
            {
                Debug.Log("Ad not ready: session cooldown");
                return false;
            }

            // 3. Проверка времени с последнего показа
            if (PlayerPrefs.HasKey(LastADKey))
            {
                long lastAdTicks = long.Parse(PlayerPrefs.GetString(LastADKey));
                DateTime lastAdTime = new DateTime(lastAdTicks, DateTimeKind.Utc);

                if ((currentTime - lastAdTime) < adCooldown)
                {
                    Debug.Log("Ad not ready: interval cooldown");
                    return false;
                }
            }

            return true;
        }


        /*[SerializeField] private ADS _ads;

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
        }*/
    }
}