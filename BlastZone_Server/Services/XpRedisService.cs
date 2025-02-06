using LobbyServerShenkar.Interfaces;

namespace LobbyServerShenkar.Services
{
    public class XpRedisService : IXpRedisService
    {
        private readonly IRedisBaseService _redisBaseService;
        public XpRedisService(IRedisBaseService redisBaseService)
        {
            _redisBaseService = redisBaseService;
        }

        public string GetPlayerXp(string key)
        {
            return _redisBaseService.GetString(key + "#Xp");
        }

        public void SetPlayerXp(string key, string Xp)
        {
            _redisBaseService.SetString(key + "#Xp", Xp);
        }
    }
}

