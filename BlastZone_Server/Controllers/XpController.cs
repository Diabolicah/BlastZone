using LobbyServerShenkar.Interfaces;
using LobbyServerShenkar.Services;
using Microsoft.AspNetCore.Mvc;

namespace LobbyServerShenkar.Controllers
{
    [Route("api/")]
    [ApiController]
    public class XpController : Controller
    {
        // private int maxLevel = 5;
        // private int maxXp = 1225;
        private readonly IXpRedisService _xpRedisService;
        private readonly LevelService _levelService;
        public XpController(IXpRedisService xpRedisService, LevelService levelService)
        {
            _xpRedisService = xpRedisService;
            _levelService = levelService;
        }

        [HttpPost("addXp")]
        public Dictionary<string, object> addXp([FromBody] Dictionary<string, object> data)
        {
            Dictionary<string,object> result = new Dictionary<string,object>();
            if(data.ContainsKey("Email") && data.ContainsKey("XpAmount"))
            {
                string email = data["Email"].ToString();
                int xp = int.Parse(data["XpAmount"].ToString());
                string xpData = _xpRedisService.GetPlayerXp(email);
                string lastLevel = _levelService.GetPlayerLevel(int.Parse(xpData));
                int curXp = int.Parse(xpData) + xp;
                string curLevel = _levelService.GetPlayerLevel(curXp);
                _xpRedisService.SetPlayerXp(email, curXp.ToString());
                result.Add("IsSuccess", true);
                result.Add("CurrentXp", curXp);
                result.Add("Rank", curLevel);
            }
            else
            {
                result.Add("IsSuccess", false);
                result.Add("ErrorMessage", "Missing Variables");
            }

            result.Add("Response", "addXp");
            return result;
        }
    }
}