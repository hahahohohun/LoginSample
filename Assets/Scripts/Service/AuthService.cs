using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using LoginSystem.Service;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using LoginSystem.Core;
using UnityEditor.Playables;

namespace LoginSystem.Service
{
    public class AuthService : IAuthService
    {
        private readonly FirebaseAuth _auth;
        private readonly IEnvironmentService _envServiceType;

        public AuthService(IEnvironmentService envServiceType)
        {
            _envServiceType = envServiceType;
            _auth = FirebaseAuth.DefaultInstance;
        }

        public async UniTask<LoginData> LoginAsync(string username, string password)
        {
            string baseUrl = _envServiceType.GetBaseUrl();
            Debug.Log($"로그인 시도 url: {baseUrl}");
            LoginData loginData = new LoginData();

            if (string.IsNullOrEmpty(username) || (string.IsNullOrEmpty(password)))
            {
                loginData.IsSuccess = false;
                loginData.ErrorMessage = "Input value IsNullOrEmpty";
                return loginData;
            }

            //회원가입
            await AuthCreate(loginData, username, password);

            //token = await AuthLogin(username, password);

            return loginData;// "Real_Token";
        }

        public void Logout()
        {
            _auth.SignOut();
        }

        private async UniTask<LoginData> AuthCreate(LoginData loginData, string username, string password)
        {
            loginData.AuthResultState = AuthState.Create;
            loginData.ErrorMessage = string.Empty;
            loginData.IsSuccess = false;

            //이메일 가입
            try
            {
                var result = await _auth
                    .CreateUserWithEmailAndPasswordAsync(username, password)
                    .AsUniTask();

                Debug.Log($"계정 생성 성공: {result.User.UserId}");
                loginData.IsSuccess = true;
                FirebaseUser newUser = result.User;

            }
            catch (OperationCanceledException)
            {
                Debug.Log("계정 생성 취소됨");
            }
            catch (FirebaseException e) //실패
            {
                Debug.LogError($"Firebase 회원가입 오류: cod : {e.ErrorCode}, msg : {e.Message}");
                loginData.ErrorMessage = Utils.GetAuthErrorMessage(e.ErrorCode);
            }

            return loginData;
        }

        private async UniTask<LoginData> AuthLogin(LoginData loginData, string username, string password)
        {
            loginData.AuthResultState = AuthState.Login;
            loginData.ErrorMessage = string.Empty;
            loginData.IsSuccess = false;

            //이메일 로그인
            try
            {
                var result = await _auth
                    .SignInWithEmailAndPasswordAsync(username, password)
                    .AsUniTask();

                Debug.Log($"계정 로그인 성공: {result.User.UserId}");
                loginData.IsSuccess = true;
                FirebaseUser newUser = result.User;

            }
            catch (OperationCanceledException)
            {
                Debug.Log("로그인 취소됨");
            }
            catch (FirebaseException e) //실패
            {
                Debug.LogError($"Firebase 로그인 오류: {e.ErrorCode}");
                loginData.ErrorMessage = Utils.GetAuthErrorMessage(e.ErrorCode);
            }

            return loginData;
        }
    }
    
    public class MockService : IAuthService
    {
        private readonly IEnvironmentService _envServiceType;
        public MockService(IEnvironmentService envServiceType)
        {
            _envServiceType = envServiceType;
        }
        
        public async UniTask<LoginData> LoginAsync(string username, string password)
        {
            LoginData loginData = new LoginData();
            string baseUrl = _envServiceType.GetBaseUrl();
            Debug.Log($"로그인 시도 url: {baseUrl}");
            
            await UniTask.Delay(1000);

            if (string.IsNullOrEmpty(username) || (string.IsNullOrEmpty(password)))
            {
                loginData.IsSuccess = false;
                loginData.ErrorMessage = "Input IsNullOrEmpty";
            }

            string token = await UniTask.RunOnThreadPool(() =>
            {
                var id = Thread.CurrentThread.ManagedThreadId;

                // 무거운 연산 시뮬레이션
                long sum = 0;
                for (int i = 0; i < 50_000_000; i++)
                    sum += i;

                loginData.IsSuccess = true;
                return $"Mock_Token{username}_{id}_{sum}";
            });

            return loginData;
        }

        public void Logout()
        {
            //
        }
    }
    
}

