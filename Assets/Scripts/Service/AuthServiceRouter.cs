using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LoginSystem.Interface;

namespace LoginSystem.Service
{
// 예시: IAuthService만 환경별로 바뀌는 경우
    public sealed class AuthServiceRouter : IAuthService
    {
        private readonly IEnvironmentService _env;
        private readonly AuthService _real;   // LIVE용
        private readonly MockService _mock;   // DEV/QA용

        public AuthServiceRouter(IEnvironmentService env, AuthService real, MockService mock)
        {
            _env = env;
            _real = real;
            _mock = mock;
        }

        private IAuthService Impl =>
            _env.Environment == ServerEnv.LIVE ? (IAuthService)_real : _mock;

        // 아래부터 IAuthService 메서드를 전부 Impl로 위임
        public UniTask<string> LoginAsync(string id, string pw) => Impl.LoginAsync(id, pw);
        
    }

}