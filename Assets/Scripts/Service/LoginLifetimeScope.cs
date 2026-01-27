using Firebase.Auth;
using LoginSystem.Service;
using LoginSystem.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LoginLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        var initialEnv = ServerEnv.DEV; // 기본값

        // EnvironmentService 싱글톤 등록
        var envService = new EnvironmentService(initialEnv); //객체 생성을 초기에 내가정하기위함
        builder.RegisterInstance<IEnvironmentService>(envService);
        builder.RegisterInstance(initialEnv); //ServerEnv가 필요한 경우는 무조건 설정한걸로 

        builder.Register<AuthService>(Lifetime.Singleton);  //인터페이스로 감쌀필요없음
        builder.Register<MockService>(Lifetime.Singleton); //인터페이스로 감쌀필요없음

        builder.Register<IAuthService, AuthServiceRouter>(Lifetime.Singleton); //객체는 AuthServiceRouter
        builder.Register<IUserService, UserService>(Lifetime.Scoped);

        
        builder.Register<LoginWork>(Lifetime.Transient);
        builder.Register<LogoutWork>(Lifetime.Transient);

        //mono
        builder.RegisterComponentInHierarchy<LoginPanel>();
        builder.RegisterComponentInHierarchy<UIHUDInfo>();
    }
}
