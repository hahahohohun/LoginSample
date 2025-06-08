using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LoginSystem.Inerface;
using LoginSystem.Service;
using UnityEngine;
 
namespace LoginSystem.Service
{
    public class AuthService : IAuthService
    {
        public async UniTask<string> LoginAsync(string username, string password)
        {
            Debug.Log("로그인 시도");
            await UniTask.Delay(1000);

            if (username.ToLower() == "fail")
                return String.Empty;
            
            return "Test_Token";
        }
    }
}

