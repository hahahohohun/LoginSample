using LoginSystem.Inerface;
using LoginSystem.Service;
using LoginSystem.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LoginLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IAuthService, AuthService>(Lifetime.Scoped);
        builder.Register<IUserService, UserService>(Lifetime.Scoped);

        builder.Register<LoginWork>(Lifetime.Transient);
        builder.Register<LogoutWork>(Lifetime.Transient);

        builder.RegisterComponentInHierarchy<LoginPanel>();
        
        Debug.Log("Configure");
    }
    
    
}
