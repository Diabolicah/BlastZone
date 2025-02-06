using System.Reflection.Metadata.Ecma335;
using LobbyServerShenkar.Interfaces;
using LobbyServerShenkar.Services;
using Microsoft.AspNetCore.Mvc;

namespace LobbyServerShenkar.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoginController
    {
        private readonly IPlayersRedisService _playersRedisService;
        private readonly IXpRedisService _xpRedisService;
        private readonly LevelService _levelService;

        public LoginController(IPlayersRedisService playersRedisService,
             IXpRedisService xpRedisService
             , LevelService levelService)
        {
            _playersRedisService = playersRedisService;
            _xpRedisService = xpRedisService;
            _levelService = levelService;
        }
        
        [HttpGet("Login/{email}&{password}")]   
        public Dictionary<string, object> Login(string email,string password) 
        {
            Dictionary<string,object> result = new Dictionary<string,object>();
            Dictionary<string, string> playerData = _playersRedisService.GetPlayer(email);
            if (playerData.Count > 0 && playerData.ContainsKey("Password") && 
                playerData.ContainsKey("UserId"))
            {
                string playerDataPassword = playerData["Password"];
                string userId = playerData["UserId"];
                string curXp = _xpRedisService.GetPlayerXp(email);
                
                
                
                if (password == playerDataPassword)
                {
                    result.Add("IsLoggedIn", true);
                    result.Add("UserId", userId);
                    result.Add("XpAmount", curXp);
                    result.Add("Rank", _levelService.GetPlayerLevel(int.Parse(curXp)));
                }
                else
                {
                    result.Add("IsLoggedIn", false);
                    result.Add("ErrorMessage", "Wrong Password");
                }
            }
            else
            {
                result.Add("IsLoggedIn", false);
                result.Add("ErrorMessage", "Player Doesnt Exist Or Missing Variables");
            }

            result.Add("Response", "Login");
            return result;
        }
    }
}
