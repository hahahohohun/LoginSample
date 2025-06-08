using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LoginSystem.Inerface;
using LoginSystem.Service;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

public class LoginTest : MonoBehaviour
{
    public void OnClick()
    {
        onLogin_Test();
    }
    
    private async UniTask onLogin_Test()
    {
        var builder = new ContainerBuilder();
        builder.Register<IAuthService, AuthService>(Lifetime.Singleton);
        builder.Register<IUserService, UserService>(Lifetime.Singleton);
        builder.Register<LoginManager>(Lifetime.Singleton);

        var container = builder.Build();
        var loginManager = container.Resolve<LoginManager>();

        var result = await loginManager.Login("test", "pw");

        Assert.IsTrue(result);
    }
}

public class LoginManager
{
    IAuthService _authService;
    public LoginManager(IAuthService authService)
    {
        _authService = authService;
    }

    public async UniTask<bool> Login(string username, string password)
    {
        return false;
    }
}

