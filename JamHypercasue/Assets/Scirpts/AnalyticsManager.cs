using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using Unity.Services.Authentication;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    private string sessionId;
    private DateTime sessionStartTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeServices();
    }

    private async void InitializeServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
            // Optionnel : authentifier un utilisateur anonyme pour suivre par joueur
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            // Demander ou vérifier le consentement utilisateur ici
            // Si le joueur consent, on active
            AnalyticsService.Instance.StartDataCollection();

            StartSession();
        }
        catch (Exception e)
        {
            Debug.LogError($"Analytics init failed: {e}");
        }
    }


    private void Start()
    {
        StartSession();
    }

    private void OnApplicationQuit()
    {
        EndSession();
    }

    private void StartSession()
    {
        int lastConnexionOffset = -1;
        if (PlayerPrefs.HasKey("LastCoDay"))
        {
            lastConnexionOffset = DateTime.UtcNow.DayOfYear - PlayerPrefs.GetInt("LastCoDay");
        }
        PlayerPrefs.SetInt("LastCoDay", DateTime.UtcNow.DayOfYear);
        
        CustomEvent e = new CustomEvent("session_start")
        {
            {"days_since_last_login", lastConnexionOffset}
        };
        
        AnalyticsService.Instance.RecordEvent(e);
        AnalyticsService.Instance.Flush();
    }

    private void EndSession()
    {
        DateTime endTime = DateTime.UtcNow;
        double duration = (endTime - sessionStartTime).TotalSeconds;
        
        CustomEvent e = new CustomEvent("session_end")
        {
            { "duration_seconds", duration },
        };
        AnalyticsService.Instance.RecordEvent(e);
        
    }

    public void AdForFreeSoft()
    {
        AnalyticsService.Instance.RecordEvent("softcurrency_rewarded_ad");
        AnalyticsService.Instance.Flush();
    }

    public void OnLevelPass(int levelNumber, bool hadSkiped)
    {
        CustomEvent e = new CustomEvent("level_passed")
        {
            {"level_id", levelNumber},
            {"has_skipped_level", hadSkiped},
        };
        AnalyticsService.Instance.RecordEvent(e);
        AnalyticsService.Instance.Flush();
    }
    
}
