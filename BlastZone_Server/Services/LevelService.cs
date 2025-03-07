
namespace LobbyServerShenkar.Services
{
    public class LevelService
    {
        public LevelService() { }
        public string GetPlayerLevel(int xp)
        {
            return (xp / 100).ToString();
        }
    }
}
