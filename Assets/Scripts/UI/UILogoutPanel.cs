using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LoginSystem.Core;
using LoginSystem.Service;
using UnityEngine;
using UnityEngine.UI;

public class UILogoutPanel : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private Text loginInfoText;
    
    private void Awake()
    {
        loginButton.onClick.AddListener(OnLogout);

        var loginInfo = LoginController.GetLoginResult();
        if (loginInfo != null)
        {
            var data = loginInfo.GetUserData();
            if (data != null)
            {
                loginInfoText.text = String.Format($"ID:{data.ID}\n닉네임:{data.NickName}\n레벨:{data.Level}");
            }
        }
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
