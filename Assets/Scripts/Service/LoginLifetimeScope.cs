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
#if UNITY_EDITOR || DEV
        env = ServerEnv.DEV;
#elif QA
        env = ServerEnv.QA;
#else
        env = ServerEnv.LIVE;
#endif
        //EnvironmentService 인스턴스를 생성
        var envService = new EnvironmentService(env);
        
        //IEnvironmentService 등록 (전체 서비스로 쓰는 용도)
        builder.RegisterInstance<IEnvironmentService>(envService);
        
        //enum 값만 따로 등록
        builder.RegisterInstance(env); 

        //.. AuthService
        if (env == ServerEnv.LIVE)
        {
            builder.Register<IAuthService, AuthService>(Lifetime.Scoped);
            builder.Register<IUserService, UserService>(Lifetime.Scoped);
        } 
        //MockService
        else if(env == ServerEnv.DEV || env == ServerEnv.QA)
        {
            builder.Register<IAuthService, MockService>(Lifetime.Scoped);
            builder.Register<IUserService, UserService>(Lifetime.Scoped);
        }

        builder.Register<LoginWork>(Lifetime.Transient);
        builder.Register<LogoutWork>(Lifetime.Transient);

        //mono
        builder.RegisterComponentInHierarchy<LoginPanel>();
        builder.RegisterComponentInHierarchy<UIHUDInfo>();
        
        Debug.Log("Configure");
    }
}
