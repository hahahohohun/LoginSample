using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LoginSystem.Core;
using LoginSystem.Interface;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginWork : IWork<LoginParam, LoginResult>
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private LoginResult _loginResult;

    public LoginWork(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    public async UniTask<LoginResult> ExecuteWorkAsync(LoginParam param)
    {
        Debug.Log("로그인 시작");

        string token = await _authService.LoginAsync(param.UserID, param.Password);
        if (token.IsUnityNull())
        {
            return new LoginResult(null, LoginErrorCode.InvalidToken, "Invalid Token");
        }

        UserData user = await _userService.LoadUserDataAsync(param.UserID, token);
        if (user.IsUnityNull())
        {
            return new LoginResult(null, LoginErrorCode.InvalidUserData,"Invalid UserData" );
        }
        
        await UniTask.Delay(500);
        
        Debug.Log($"로그인 성공 : {user.NickName}, {user.Level}");
        return _loginResult = new LoginResult(user, LoginErrorCode.None,"Success" );
    }
}

public class LogoutWork : IWork
{
    public async UniTask ExecuteWorkAsync()
    {
        Debug.Log("로그아웃 시작");
        
        // 세션 초기화, 캐시 삭제 등
        await UniTask.Delay(500);

        Debug.Log("[LogoutWork] 로그아웃 완료");
        
        await SceneManager.LoadSceneAsync("LoginScene").ToUniTask();
    }
}
