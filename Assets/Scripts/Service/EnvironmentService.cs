namespace LoginSystem.Service
{
    public class EnvironmentService : IEnvironmentService
    {
        private readonly ServerEnv _env;

        public EnvironmentService()
        {
#if UNITY_EDITOR
            _env = ServerEnv.DEV; // 또는 에디터 선택값
#elif DEV
        _env = ServerEnv.DEV;
#elif QA
        _env = ServerEnv.QA;
#else
        _env = ServerEnv.LIVE;
#endif
        }

        public ServerEnv Environment => _env;

        public string GetBaseUrl()
        {
            return _env switch
            {
                //임시용 주소
                ServerEnv.DEV => "https://dev.api.game.com",
                ServerEnv.QA => "https://qa.api.game.com",
                ServerEnv.LIVE => "https://live.api.game.com",
                _ => "https://dev.api.game.com"
            };
        }
    }
}