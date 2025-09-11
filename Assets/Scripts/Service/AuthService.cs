using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LoginSystem.Interface;
using LoginSystem.Service;
using UnityEngine;
 
namespace LoginSystem.Service
{
    public class AuthService : IAuthService
    {
        private readonly IEnvironmentService _envServiceType;

        public AuthService(IEnvironmentService envServiceType)
        {
            _envServiceType = envServiceType;
        }
        
        public async UniTask<string> LoginAsync(string username, string password)
        {
            string baseUrl = _envServiceType.GetBaseUrl();
            Debug.Log($"로그인 시도 url: {baseUrl}");
            
            await UniTask.Delay(1000);

            if (username.ToLower() == "fail")
                return String.Empty;
            
            return "Real_Token";
        }
    }
    
    //샘플코드니까 일단 같은 cs파일에 배치
    public class MockService : IAuthService
    {
        private readonly IEnvironmentService _envServiceType;
        public MockService(IEnvironmentService envServiceType)
        {
            _envServiceType = envServiceType;
        }
        
        public async UniTask<string> LoginAsync(string username, string password)
        {
            string baseUrl = _envServiceType.GetBaseUrl();
            Debug.Log($"로그인 시도 url: {baseUrl}");
            
            await UniTask.Delay(1000);

            if (username.ToLower() == "fail")
                return String.Empty;
            
            return "Mock_Token";
        }
    }
    
}

