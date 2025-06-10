namespace LoginSystem.Service
{
    public class EnvironmentService : IEnvironmentService
    {
        private readonly ServerEnv _env;

        public EnvironmentService(ServerEnv evn)
        {
            _env = evn;
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