using System.Collections;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;
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

		var initialEnv = ServerEnv.DEV; // DEV/QA면 Mock, LIVE면 Real 쓰도록 가정
		var envService = new EnvironmentService(initialEnv);
		builder.RegisterInstance<IEnvironmentService>(envService);
		builder.RegisterInstance(initialEnv); // (선택) enum 필요시

		builder.Register<AuthService>(Lifetime.Singleton);   // LIVE 용
		builder.Register<MockService>(Lifetime.Singleton);   // DEV/QA 용

		builder.Register<IAuthService, AuthServiceRouter>(Lifetime.Singleton);

		builder.Register<IUserService, UserService>(Lifetime.Singleton);
		builder.Register<LoginWork>(Lifetime.Transient);
		builder.Register<LogoutWork>(Lifetime.Transient);

		builder.Register<TestLoginManager_Btn>(Lifetime.Singleton);

		var container = builder.Build();
		var loginManager = container.Resolve<TestLoginManager_Btn>();

		bool result = await loginManager.Login_Succed("test", "pw");
		Assert.IsTrue(result);
		Debug.Log($"[LoginTest] Assert passed {result}");
	}
}

public class TestLoginManager_Btn
{
    private IAuthService _authService;
    public TestLoginManager_Btn(IAuthService authService)
    {
        _authService = authService;
    }

	public async UniTask<bool> Login_Succed(string username, string password)
    {
        var res = await _authService.LoginAsync(username, password);
        return !string.IsNullOrEmpty(res);

    }
}

