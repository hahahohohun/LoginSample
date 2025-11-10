using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LoginSystem.Core
{
    public static class LoginController
    {
        private static LoginResult _loginResult;

        public static void CacheLoginResult(LoginResult loginResult)
        {
            _loginResult = loginResult;
        }

        public static LoginResult GetLoginResult()
        {
            return _loginResult;
        }
    }
    public class WorkExecutor<TParm, TResult>
    {
        private readonly IWork<TParm, TResult> _work;

        public WorkExecutor(IWork<TParm, TResult> work)
        {
            _work = work;
        }

        public async UniTask<TResult> RunAsync(TParm parm)
        {
            try
            {
                return await _work.ExecuteWorkAsync(parm);
            }
            catch (Exception e)
            {
                Debug.LogError($"[WorkExecutor] 실패: {e.Message}");
                throw;
            }
        }
    }

    public class WorkExecutor
    {
        private readonly IWork _work;

        public WorkExecutor(IWork work)
        {
            _work = work;
        }

        public async UniTask RunAsync()
        {
            try
            {
                await _work.ExecuteWorkAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($"[WorkExecutor] 실패: {e.Message}");
            }
        }
    }

    public class LoginResult
    {
        public bool IsSuccess => ErrorCode == LoginErrorCode.None;
        public LoginErrorCode ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }

        private UserData _userData;

        public UserData GetUserData()
        {
            return _userData;
        }

        public LoginResult(UserData userData, LoginErrorCode errorCode, string errorMessage)
        {
            _userData = userData;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}


