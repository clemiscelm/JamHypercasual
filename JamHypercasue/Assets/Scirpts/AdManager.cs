using System;
using System.Collections;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdManager Instance;
    [SerializeField] private bool _isTesting;
    private const string _androidAdUnitId = "Interstitial_Android";
    private const string _androidAdRewaredUnitId = "Rewarded_Android";
    private const string _gameId = "5978447";

    private bool _isDisplayingAd;
    public bool AdDisplaying => _isDisplayingAd;
    private void AdDebug(string message) => Debug.Log($"[ADS]: {message}");
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Advertisement.Initialize(_gameId, _isTesting, this);
    }
    

    public void LoadAd()
    {
        Advertisement.Load(_androidAdUnitId, this);
    }

    public void ShowAd()
    {
        if(Advertisement.isShowing)
            return;
        
        Advertisement.Show(_androidAdUnitId, this);
        _isDisplayingAd = true;
    }
    
    public void ShowRewardedAd()
    {
        if(Advertisement.isShowing)
            return;
        
        Advertisement.Show(_androidAdRewaredUnitId, this);
        _isDisplayingAd = true;
    }

    private void RecordAdImpression(string placementId, UnityAdsShowCompletionState state)
    {
        try
        {
            // Remplir selon la doc standard de Unity Analytics
            var adImpression = new AdImpressionEvent
            {
                AdProvider = AdProvider.UnityAds,
                PlacementId = placementId,
                PlacementName = placementId, 
                PlacementType = placementId.Equals(_androidAdRewaredUnitId, StringComparison.OrdinalIgnoreCase)
                    ? AdPlacementType.REWARDED
                    : AdPlacementType.INTERSTITIAL,
                AdCompletionStatus = ConvertAdState(state)
            };

            AnalyticsService.Instance.RecordEvent(adImpression);
            AnalyticsService.Instance.Flush();
            AdDebug($"Recorded adImpression for placement {placementId}");
        }
        catch (Exception e)
        {
            AdDebug($"Failed to record adImpression: {e}");
        }
    }

    private AdCompletionStatus ConvertAdState(UnityAdsShowCompletionState s)
    {
        switch (s)
        {
            case UnityAdsShowCompletionState.SKIPPED:
                return AdCompletionStatus.Partial;
            case UnityAdsShowCompletionState.COMPLETED:
                return AdCompletionStatus.Completed;
            case UnityAdsShowCompletionState.UNKNOWN:
                return AdCompletionStatus.Incomplete;
            default:
                throw new ArgumentOutOfRangeException(nameof(s), s, null);
        }
    }
    
    public void OnUnityAdsAdLoaded(string placementId)
    {
        AdDebug($"Ad {placementId} loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        AdDebug($"{error}: \n placement id : {placementId} \n message: {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        AdDebug($"{error}: \n placement id : {placementId} \n message: {message}");

    }
    public void OnUnityAdsShowStart(string placementId)
    {
        AdDebug($"Displaying placement {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        AdDebug($"Placement {placementId} has been clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        AdDebug($"Placement {placementId}  state is : {showCompletionState}");
        RecordAdImpression(placementId, showCompletionState);
        _isDisplayingAd = false;
    }

    public void OnInitializationComplete()
    {
        AdDebug($"Initialisation Compeleted");
        LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        AdDebug($"{error} \n {message}");
    }
}
