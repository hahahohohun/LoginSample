using System;
using Cysharp.Threading.Tasks;
using LoginSystem.Inerface;
using UnityEngine;

namespace LoginSystem.Service
{
    public class UserService : IUserService
    {
        public async UniTask<UserData> LoadUserDataAsync(string token)
        {
            Debug.Log($"[MockUser] 유저 데이터 로딩 시작 (token: {token})");

            await UniTask.Delay(1000); // 서버 대기 시뮬레이션

            if (string.IsNullOrEmpty(token))
                return null;

            var userData = new UserData
            {
                NickName = "테스트유저",
                Level = UnityEngine.Random.Range(1, 100)
            };

            Debug.Log($"[MockUser] 유저 데이터 로딩 완료: {userData.NickName}, Lv.{userData.Level}");

            return userData;
        }
    }

}
