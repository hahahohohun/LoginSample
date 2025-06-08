using System;
using Cysharp.Threading.Tasks;
using LoginSystem.Inerface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoginSystem.Core
{
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

        public LoginResult(LoginErrorCode errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}


