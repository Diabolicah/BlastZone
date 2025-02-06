using LobbyServerShenkar.Interfaces;

namespace LobbyServerShenkar.Services
{
    public class PlayersRedisService : IPlayersRedisService
    {
        private readonly IRedisBaseService _redisBaseService;
        public PlayersRedisService(IRedisBaseService redisBaseService)
        {
            _redisBaseService = redisBaseService;
        }

        public Dictionary<string, string> GetPlayer(string key)
        {
            return _redisBaseService.GetDictionary(key + "#Players");
        }

        public void SetPlayer(string key, Dictionary<string, string> playerData)
        {
           _redisBaseService.SetDictionary(key + "#Players", playerData);    
        }
    }
}
