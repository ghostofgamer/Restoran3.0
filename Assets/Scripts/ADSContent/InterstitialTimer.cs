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
        private bool _showInter= false;

        private void Start()
        {
            lastAdTime = DateTime.Now;
        }

        private void Update()
        {
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

        private void ShowInterstitial()
        {
            _ads.ShowInterstitial();
        }
    }
}