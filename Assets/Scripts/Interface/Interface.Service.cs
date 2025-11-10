using Cysharp.Threading.Tasks;

public interface IEnvironmentService
{
	ServerEnv Environment { get; }
	event System.Action<ServerEnv> OnChanged;
	void Set(ServerEnv env);
	string GetBaseUrl(); //접속 주소 Url
}

public interface IAuthService
{
	UniTask<string> LoginAsync(string username, string password);
}

public interface IUserService
{
	UniTask<UserData> LoadUserDataAsync(string id, string token);
}

public class UserData
{
	public string ID;
	public string NickName;
	public int Level;
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
