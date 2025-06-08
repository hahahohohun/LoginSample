using Cysharp.Threading.Tasks;

namespace LoginSystem.Inerface
{
    public interface IAuthService
    {
        UniTask<string> LoginAsync(string username, string password);
    }
    
    public interface IUserService
    {
        UniTask<UserData> LoadUserDataAsync(string token);
    }

    public class UserData
    {
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
        public string Username;
        public string Password;
    }
}

