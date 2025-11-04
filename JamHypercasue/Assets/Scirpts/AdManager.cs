using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdManager Instance;
    [SerializeField] private bool _isTesting;
    private const string _androidAdUnitId = "Interstitial_Android";
    private const string _gameId = "5978447";
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
        
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _isTesting, this);
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);
        ShowAd();
        yield return new WaitForSeconds(5f);
        ShowAd();
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
