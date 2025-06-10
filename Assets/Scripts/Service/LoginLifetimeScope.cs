using LoginSystem.Inerface;
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
        //인하우스서버.. AuthService
        if (InHouseServer)
        {
            builder.Register<IAuthService, AuthService>(Lifetime.Scoped);
            builder.Register<IUserService, UserService>(Lifetime.Scoped);
        } 
        else //로컬서버.. MockService
        {
            builder.Register<IAuthService, MockService>(Lifetime.Scoped);
            builder.Register<IUserService, UserService>(Lifetime.Scoped);
        }

        builder.Register<IEnvironmentService, EnvironmentService>(Lifetime.Scoped);
        
        builder.Register<LoginWork>(Lifetime.Transient);
        builder.Register<LogoutWork>(Lifetime.Transient);

        //mono
        builder.RegisterComponentInHierarchy<LoginPanel>();
        
        Debug.Log("Configure");
    }
}
