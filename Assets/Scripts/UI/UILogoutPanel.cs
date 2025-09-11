using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LoginSystem.Core;
using LoginSystem.Interface;
using LoginSystem.Service;
using UnityEngine;
using UnityEngine.UI;

public class UILogoutPanel : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    private void Awake()
    {
        loginButton.onClick.AddListener(OnLogout);
    }

    private void OnLogout()
    {
        onLogout();
    }

    private async void onLogout()
    {
        var logoutExecutor = new WorkExecutor(new LogoutWork());
        await logoutExecutor.RunAsync();
    }
}
