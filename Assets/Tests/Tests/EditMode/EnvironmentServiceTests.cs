using System.Collections;
using Cysharp.Threading.Tasks;

using LoginSystem.Service;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VContainer;

public class EnvironmentServiceTests
{
    [Test]
    public void DefaultEnv_Is_DEV()
    {
        var env = new EnvironmentService(ServerEnv.DEV);
        Assert.AreEqual(ServerEnv.DEV, env.Environment);
    }

    //환경 변경 체크
    [Test]
    public void Router_Env_Change()
    {
        //var env = new EnvironmentService(ServerEnv.DEV);
        //var router = new AuthServiceRouter(env, new AuthService(env, Fire ), new MockService(env));
        //Assert.IsInstanceOf<MockService>(router.Service);
        //env.Set(ServerEnv.LIVE);
        //Assert.IsInstanceOf<AuthService>(router.Service);
    }


    //잘못된 DI 등록(중복,순서)
    [Test]
    public void Container_Resolves_Router_Only()
    {
        var builder = new ContainerBuilder();
        builder.Register<AuthService>(Lifetime.Singleton);

        //중복등록하면 실패됨
        //builder.Register<IAuthService, AuthServiceRouter>(Lifetime.Singleton);

        builder.Register<MockService>(Lifetime.Singleton);
        builder.Register<IAuthService, AuthServiceRouter>(Lifetime.Singleton);
        var envService = new EnvironmentService(ServerEnv.DEV);
        builder.RegisterInstance<IEnvironmentService>(envService);
        var container = builder.Build();

        Assert.IsInstanceOf<AuthServiceRouter>(container.Resolve<IAuthService>());
    }
}
