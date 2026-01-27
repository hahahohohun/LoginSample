using Cysharp.Threading.Tasks;
using LoginSystem.Core;

public interface IEnvironmentService
{
	ServerEnv Environment { get; }
	event System.Action<ServerEnv> OnChanged;
	void Set(ServerEnv env);
	string GetBaseUrl(); //접속 주소 Url
}

public interface IAuthService
{
	UniTask<LoginData> LoginAsync(string username, string password);
	void Logout();
}

public interface IUserService
{
	UniTask<UserData> LoadUserDataAsync(string id, LoginData token);
}

public class UserData
{
	public string ID;
	public string NickName;
	public int Level;
	public string ErrorMessage;
}

//
public interface IWork<TParm, TResult>
{
	UniTask<TResult> ExecuteWorkAsync(TParm parm);
}

public interface IWork
{
	UniTask ExecuteWorkAsync();
}

public struct LoginParam
{
	public string UserID;
	public string Password;
}
