using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LoginSystem.Service
{
    public class UserService : IUserService
    {
        public async UniTask<UserData> LoadUserDataAsync(string id, string token)
        {
            Debug.Log($"[MockUser] 유저 데이터 로딩 시작 (token: {token})");

            await UniTask.Delay(1000); // 서버 대기 시뮬레이션

            if (string.IsNullOrEmpty(token))
                return null;

            UserData userData = null;
            await UniTask.RunOnThreadPool(() =>
            {
                // 무거운 연산 시뮬레이션
                long sum = 0;
                for (int i = 0; i < 50_000_000; i++)
                    sum += i;

                userData = new UserData
                {
                    ID = id,
                    NickName = "테스트유저",
                    
                };

            });

            if (userData != null)
            {
                //메인스레드에서 처리
                userData.Level = UnityEngine.Random.Range(1, 100);
            }

            Debug.Log($"[MockUser] 유저 데이터 로딩 완료: {userData.NickName}, Lv.{userData.Level}");

            return userData;
        }
    }

}
