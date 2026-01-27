using System.Collections;
using Cysharp.Threading.Tasks;

using LoginSystem.Service;
//using LoginSystem.Interface;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VContainer;

public class LoginPlayModeTests
{
	private IObjectResolver _container;

	[SetUp]
	public void Setup()
	{
		var builder = new ContainerBuilder();

		var envService = new EnvironmentService(ServerEnv.DEV);
		builder.RegisterInstance<IEnvironmentService>(envService);

		builder.Register<AuthService>(Lifetime.Singleton);
		builder.Register<MockService>(Lifetime.Singleton);
		builder.Register<IAuthService, AuthServiceRouter>(Lifetime.Singleton);
		builder.Register<IUserService, UserService>(Lifetime.Singleton);
		builder.Register<LoginWork>(Lifetime.Transient);

		_container = builder.Build();
	}

	[UnityTest]
	public IEnumerator Login_Succeed_Test() => UniTask.ToCoroutine(async () =>
	{
		var mgr = new TestLoginManager(_container.Resolve<IAuthService>());
		bool result = await mgr.Login_Test("test", "pw");
		Assert.IsTrue(result, "Mock 로그인은 성공해야 합니다.");
	});

    //아이디/패스워드 공백
    [UnityTest]
	public IEnumerator Login_Failed_Test_IDPW_Empty() => UniTask.ToCoroutine(async () =>
	{
		var mgr = new TestLoginManager(_container.Resolve<IAuthService>());
		//잘못된 파라미터 전달
        bool result = await mgr.Login_Test(string.Empty, string.Empty);

        Assert.IsFalse(result, "Mock 로그인은 실패해야 합니다.");
	});

    //아이디 공백
    [UnityTest]
    public IEnumerator Login_Failed_ID_Empty() => UniTask.ToCoroutine(async () =>
    {
        var mgr = new TestLoginManager(_container.Resolve<IAuthService>());
        //잘못된 파라미터 전달
        bool result = await mgr.Login_Test(string.Empty, "123");

        Assert.IsFalse(result, "Mock 로그인은 실패해야 합니다.");
    });

	//패스워드 공백
    [UnityTest]
    public IEnumerator Login_Failed_PW_Empty() => UniTask.ToCoroutine(async () =>
    {
        var mgr = new TestLoginManager(_container.Resolve<IAuthService>());
        //잘못된 파라미터 전달
        bool result = await mgr.Login_Test("TestID", string.Empty);

        Assert.IsFalse(result, "Mock 로그인은 실패해야 합니다.");
    });

}

public class TestLoginManager
{
	private readonly IAuthService _authService;

	public TestLoginManager(IAuthService auth)
	{
		_authService = auth;
	}

	// MockService에서 성공 케이스 호출
	public async UniTask<bool> Login_Test(string username, string password)
	{
		var res = await _authService.LoginAsync(username, password);
		return res.IsSuccess;

    }

}
