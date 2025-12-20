using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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

            if (username.ToLower() == "fail" || string.IsNullOrEmpty(username))
                return string.Empty;

            if (string.IsNullOrEmpty(password))
                return string.Empty;

            string token = await UniTask.RunOnThreadPool(() =>
            {
                var id = Thread.CurrentThread.ManagedThreadId;

                // 무거운 연산 시뮬레이션
                long sum = 0;
                for (int i = 0; i < 50_000_000; i++)
                    sum += i;

                return $"Real_Token{username}_{id}_{sum}";
            });

            return "Real_Token";
        }
    }
    
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

            if (username.ToLower() == "fail" || string.IsNullOrEmpty(username))
                return string.Empty;

            if (string.IsNullOrEmpty(password))
                return string.Empty;


            string token = await UniTask.RunOnThreadPool(() =>
            {
                var id = Thread.CurrentThread.ManagedThreadId;

                // 무거운 연산 시뮬레이션
                long sum = 0;
                for (int i = 0; i < 50_000_000; i++)
                    sum += i;

                return $"Mock_Token{username}_{id}_{sum}";
            });


            return "Mock_Token";
        }
    }
    
}

