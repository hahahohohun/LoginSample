using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LoginSystem.Core;
using Firebase;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;


public class LoginWork : IWork<LoginParam, LoginResult>
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public LoginWork(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    public async UniTask<LoginResult> ExecuteWorkAsync(LoginParam param)
    {
        Debug.Log("로그인 시작");

        var loginData = await _authService.LoginAsync(param.UserID, param.Password);
        if (loginData.IsSuccess == false)
        {
            return new LoginResult(null, loginData.AuthResultState, LoginErrorCode.InvalidToken, loginData.ErrorMessage);
        }
        
        UserData user = await _userService.LoadUserDataAsync(param.UserID, loginData);

        if (user == null)
        {
            return new LoginResult(null, loginData.AuthResultState, LoginErrorCode.InvalidUserData, user.ErrorMessage);
        }
        
        //await UniTask.Delay(500);
        
        Debug.Log($"로그인/계정생성 성공 : {user.NickName}, {user.Level}");
        return new LoginResult(user, loginData.AuthResultState, LoginErrorCode.None, string.Empty);
    }
}

public class LogoutWork : IWork
{
    private readonly IAuthService _authService;

    public LogoutWork(IAuthService authService)
    {
        _authService = authService;
    }

    public async UniTask ExecuteWorkAsync()
    {
        Debug.Log("로그아웃 시작");
        
        // 세션 초기화, 캐시 삭제 등
        _authService.Logout();

        Debug.Log("[LogoutWork] 로그아웃 완료");
        
        await SceneManager.LoadSceneAsync("LoginScene").ToUniTask();
    }
}
