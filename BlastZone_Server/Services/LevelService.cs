
namespace LobbyServerShenkar.Services
{
    public class LevelService
    {
        public LevelService() { }
        public string GetPlayerLevel(int xp)
        {
            // if (xp < 100) return "1";
            // if (xp < 300) return "2";
            // if (xp < 650) return "3";
            // if (xp < 1225) return "4";
            // return "5";
            return (xp / 100).ToString();
        }
    }
}
