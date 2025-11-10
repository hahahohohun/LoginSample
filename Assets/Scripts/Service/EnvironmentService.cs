using System;

namespace LoginSystem.Service
{
    public class EnvironmentService : IEnvironmentService
    {
        private ServerEnv _env;

        public EnvironmentService(ServerEnv evn)
        {
            _env = evn;
        }

        public ServerEnv Environment => _env;
        public event Action<ServerEnv> OnChanged;

        //ServerEnv환경에 따른 주소 리턴
        public void Set(ServerEnv env)
        {
            if (_env == env)
                return;

            _env = env;
            OnChanged?.Invoke(_env);
        }

        public string GetBaseUrl()
        {
            return _env switch
            {
                //임시용 주소
                ServerEnv.DEV => "//dev.api.game.com",
                ServerEnv.QA => "//qa.api.game.com",
                ServerEnv.LIVE => "//live.api.game.com",
                _ => "//dev.api.game.com"
            };
        }
    }
}