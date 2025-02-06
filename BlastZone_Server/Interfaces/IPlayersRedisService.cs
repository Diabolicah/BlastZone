namespace LobbyServerShenkar.Interfaces
{
    public interface IPlayersRedisService
    {
        public Dictionary<string, string> GetPlayer(string key);

        public void SetPlayer(string key, Dictionary<string, string> playerData);
    }
}
