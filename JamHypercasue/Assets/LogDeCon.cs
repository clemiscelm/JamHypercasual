using System;
using TMPro;
using UnityEngine;

public class LogDeCon : MonoBehaviour
{
    [SerializeField] private TMP_Text yexy;

    private void Awake()
    {
        Application.logMessageReceived += Prout;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= Prout;
    }

    private void Prout(string condition, string stacktrace, LogType type)
    {
        yexy.text += $"\n {condition}";
    }
}
