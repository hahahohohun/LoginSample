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
	public IEnumerator Login_Succeeds_WhenMockReturnsSuccess() => UniTask.ToCoroutine(async () =>
	{
		var mgr = new TestLoginManager(_container.Resolve<IAuthService>());
		bool result = await mgr.Login_Success("test", "pw");
		Assert.IsTrue(result, "Mock 로그인은 성공해야 합니다.");
	});

	[UnityTest]
	public IEnumerator Login_Fails_WhenMockReturnsFailure() => UniTask.ToCoroutine(async () =>
	{
		var mgr = new TestLoginManager(_container.Resolve<IAuthService>());
        bool result = await mgr.Login_Fail("", "");
		Assert.IsFalse(result, "Mock 로그인은 실패해야 합니다.");
	});
}

public class TestLoginManager
{
	private readonly IAuthService _auth;

	public TestLoginManager(IAuthService auth) => _auth = auth;

	// MockService에서 성공 케이스 호출
	public async UniTask<bool> Login_Success(string id, string pw)
	{
		var res = await _auth.LoginAsync(id, pw);
		return !string.IsNullOrEmpty(res);
	}

	// 실패 케이스 시뮬레이션
	public async UniTask<bool> Login_Fail(string id, string pw)
	{
		// 실패 상황을 흉내내기 위해 잘못된 파라미터 전달
		var res = await _auth.LoginAsync("", "");
		return !string.IsNullOrEmpty(res);
	}
}
