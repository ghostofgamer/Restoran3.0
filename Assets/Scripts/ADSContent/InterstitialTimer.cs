using System;
using UnityEngine;

namespace ADSContent
{
    public class InterstitialTimer : MonoBehaviour
    {
        [SerializeField] private ADS _ads;
        [SerializeField]private float interval; 
    
        private float timer = 0f;
        private DateTime lastAdTime;
        private bool _showInter= true;

        private void Start()
        {
            bool removeAds = PlayerPrefs.GetInt("removeADS") == 1;
            SetValue(!removeAds);
            
            lastAdTime = DateTime.Now;
        }

        private void Update()
        {
            if (!_showInter)
                return;
            
            timer += Time.unscaledDeltaTime;

            if (timer >= interval)
            {
                ShowInterstitial();
                timer = 0f;
                lastAdTime = DateTime.Now;
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                lastAdTime = DateTime.Now;
            }
            else
            {
                TimeSpan timePassed = DateTime.Now - lastAdTime;
                timer += (float)timePassed.TotalSeconds;
            }
        }

        public void SetValue(bool value)
        {
            _showInter = value;
        }

        private void ShowInterstitial()
        {
            _ads.ShowInterstitial();
        }
    }
}