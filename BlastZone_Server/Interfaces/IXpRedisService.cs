namespace LobbyServerShenkar.Interfaces
{
    public interface IXpRedisService
    {
        public string GetPlayerXp(string key);

        public void SetPlayerXp(string key, string Xp);
    }
}
