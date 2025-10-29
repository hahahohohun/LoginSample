using LoginSystem.Interface;
using LoginSystem.Service;
using LoginSystem.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LoginLifetimeScope : LifetimeScope
{
    public bool InHouseServer = true; //
    protected override void Configure(IContainerBuilder builder)
    {
        var env = ServerEnv.DEV;
//#if UNITY_EDITOR || DEV
//        env = ServerEnv.DEV;
//#elif QA
//        env = ServerEnv.QA;
//#else
//        env = ServerEnv.LIVE;
//#endif

        var initialEnv = ServerEnv.DEV; // 기본값

        // EnvironmentService 싱글톤 등록
        var envService = new EnvironmentService(initialEnv);
        builder.RegisterInstance<IEnvironmentService>(envService);
        builder.RegisterInstance(initialEnv); 
        
        //.. AuthService
        //if (env == ServerEnv.LIVE)
        //{
        //    builder.Register<IAuthService, AuthService>(Lifetime.Scoped);
        //    builder.Register<IUserService, UserService>(Lifetime.Scoped);
        //} 
        ////MockService
        //else if(env == ServerEnv.DEV || env == ServerEnv.QA)
        //{
        //    builder.Register<IAuthService, MockService>(Lifetime.Scoped);
        //    builder.Register<IUserService, UserService>(Lifetime.Scoped);
        //}
        
        builder.Register<AuthService>(Lifetime.Singleton);
        builder.Register<MockService>(Lifetime.Singleton);
        builder.Register<IAuthService, AuthServiceRouter>(Lifetime.Singleton);
        builder.Register<IUserService, UserService>(Lifetime.Scoped);

        
        builder.Register<LoginWork>(Lifetime.Transient);
        builder.Register<LogoutWork>(Lifetime.Transient);

        //mono
        builder.RegisterComponentInHierarchy<LoginPanel>();
        builder.RegisterComponentInHierarchy<UIHUDInfo>();
        
        Debug.Log("Configure");
    }
}
