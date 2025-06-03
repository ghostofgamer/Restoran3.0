using System;
using System.Collections;
using Io.AppMetrica;
using UnityEngine;
using UnityEngine.UI;

namespace ADSContent
{
    public class ADS : MonoBehaviour
    {
        public string SDKKey = "nR3VEu5EEJlq6OmqwXd1lMKHQhg3sEumJpdTgplZ-csu1yq6zIkU1auq9P1sOOoIVLg9tOWSXDaUfRvC9Uv-Ib";
        public string InterstitialKey;
        public string RewardedKey;
        public string BannerKey;
        private bool isInitialized = false;

        private RewardCallback currentRewardCallback;
        private Coroutine _reloadInterstitialCoroutine;
        private bool _isInterstitialLoading = false;
        private bool _isAdPreloading = false;
        private int _interstitialRetryAttempt = 0;
        
        public delegate void RewardCallback();

        public bool IsInterstitialReady => MaxSdk.IsInterstitialReady(InterstitialKey);

        public event Action _interHidden;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                Debug.Log("AppLovin successfully initialized");
            };

            MaxSdk.SetSdkKey(SDKKey);
            MaxSdk.InitializeSdk();

            InitializeInterstitialAds();
            InitializeRewardedAds();
        }

        public void InitializeInterstitialAds()
        {
            // Attach callback
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

            // Load the first interstitial
            LoadInterstitial();
        }

        private void LoadInterstitial()
        {
            if (_isInterstitialLoading || MaxSdk.IsInterstitialReady(InterstitialKey))
                return;

            _isInterstitialLoading = true;

            MaxSdk.LoadInterstitial(InterstitialKey);
        }

        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            _isInterstitialLoading = false;
            _interstitialRetryAttempt = 0; 
            AppMetrica.ReportEvent("OnInterstitialLoadedEvent");
        }

        private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            AppMetrica.ReportEvent("OnInterstitialLoadFailedEvent");
            _isInterstitialLoading = false;

            _interstitialRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, _interstitialRetryAttempt));
            
            if (_reloadInterstitialCoroutine != null)
                StopCoroutine(_reloadInterstitialCoroutine);

            _reloadInterstitialCoroutine = StartCoroutine(ReloadInterstitialAfterDelay((float)retryDelay));
            // LoadInterstitial();
        }

        private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            AppMetrica.ReportEvent("OnInterstitialDisplayedEvent");
        }

        private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            AppMetrica.ReportEvent("OnInterstitialAdFailedToDisplayEvent");
            // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
            
            if (_reloadInterstitialCoroutine != null)
                StopCoroutine(_reloadInterstitialCoroutine);

            _reloadInterstitialCoroutine = StartCoroutine(ReloadInterstitialAfterDelay(0.5f));
        }

        private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            AppMetrica.ReportEvent("OnInterstitialClickedEvent");
        }

        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            _interHidden?.Invoke();
            AppMetrica.ReportEvent("OnInterstitialHiddenEvent");
            // Interstitial ad is hidden. Pre-load the next ad.
            
            if (_reloadInterstitialCoroutine != null)
                StopCoroutine(_reloadInterstitialCoroutine);

            _reloadInterstitialCoroutine = StartCoroutine(ReloadInterstitialAfterDelay(0.5f));

            // LoadInterstitial();
        }

        public void ShowInterstitial()
        {
            if (MaxSdk.IsInterstitialReady(InterstitialKey))
            {
                AppMetrica.ReportEvent("ShowInterstitial");
                MaxSdk.ShowInterstitial(InterstitialKey);
            }
            else
            {
                LoadInterstitial();
            }
        }

        private IEnumerator ReloadInterstitialAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            LoadInterstitial();
        }


        public void InitializeRewardedAds()
        {
            // Attach callback
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            // Load the first rewarded ad
            LoadRewardedAd();
        }

        private void LoadRewardedAd()
        {
            MaxSdk.LoadRewardedAd(RewardedKey);
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("OnRewardedAdLoadedEvent");
            //
        }

        private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            Debug.Log("OnRewardedAdLoadFailedEvent");
            LoadRewardedAd();
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("OnRewardedAdDisplayedEvent");
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("OnRewardedAdFailedToDisplayEvent");
            // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
            LoadRewardedAd();
        }

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("OnRewardedAdClickedEvent");
        }

        private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("OnRewardedAdHiddenEvent");
            // Rewarded ad is hidden. Pre-load the next ad
            LoadRewardedAd();
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("OnRewardedAdReceivedRewardEvent");

            if (currentRewardCallback != null)
            {
                currentRewardCallback();
                currentRewardCallback = null; //// Вызываем делегат
            }
            // The rewarded ad displayed and the user should receive the reward.
        }

        private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("OnRewardedAdRevenuePaidEvent");
            // Ad revenue paid. Use this callback to track user revenue.
        }

        public void ShowRewarded(RewardCallback rewardCallback)
        {
            if (MaxSdk.IsRewardedAdReady(RewardedKey))
            {
                currentRewardCallback = rewardCallback;
                MaxSdk.ShowRewardedAd(RewardedKey);
            }
        }


        public void InitializeBannerAds()
        {
            // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
            // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
            MaxSdk.CreateBanner(BannerKey, MaxSdkBase.BannerPosition.BottomCenter);

            // Set background color for banners to be fully functional
            MaxSdk.SetBannerBackgroundColor(BannerKey, Color.clear);

            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
        }

        private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
        }

        private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
        }

        private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
        }

        private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
        }

        private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
        }

        private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
        }

        public void ShowBanner()
        {
            MaxSdk.ShowBanner(BannerKey);
        }

        public void HideBanner()
        {
            MaxSdk.HideBanner(BannerKey);
        }
    }
}