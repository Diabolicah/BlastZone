using LobbyServerShenkar.Interfaces;
using LobbyServerShenkar.Services;
using Microsoft.AspNetCore.Mvc;

namespace LobbyServerShenkar.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RegisterController : Controller
    {
        private int initialXp = 0;
        private int initialCurrency = 0;
        private int initialGems = 0;
        private readonly IPlayersRedisService _playersRedisService;
        private readonly IXpRedisService _xpRedisService;
        
        public RegisterController(IPlayersRedisService playersRedisService, 
            IXpRedisService xpRedisService)
        {
            _playersRedisService = playersRedisService;
            _xpRedisService = xpRedisService;
        }

        [HttpPost("Register")]
        public Dictionary<string, object> Register([FromBody] Dictionary<string, object> data)
        {
            Dictionary<string,object> result = new Dictionary<string,object>();
            if(data.ContainsKey("Email") && data.ContainsKey("Password"))
            {
                string? email = data["Email"].ToString();
                Dictionary<string,string> playerData = _playersRedisService.GetPlayer(email);
                if(playerData.Count == 0)
                {
                    string? password = data["Password"].ToString();
                    string userId = Guid.NewGuid().ToString();
                    Dictionary<string, string> registration = new Dictionary<string, string>()
                    {
                        { "Email",email}, 
                        { "Password",password}, 
                        { "UserId", userId },
                        {"CreatedTime",DateTime.UtcNow.ToString() }
                        
                    };
                    
                    _playersRedisService.SetPlayer(email, registration);
                    _xpRedisService.SetPlayerXp(email, initialXp.ToString());
                    result.Add("IsCreated",true);
                }
                else
                {
                    result.Add("IsCreated", false);
                    result.Add("ErrorMessage", "User Already Exist");
                }
            }
            else
            {
                result.Add("IsCreated", false);
                result.Add("ErrorMessage", "Missing Variables");
            }

            result.Add("Response", "Register");
            return result;
        }
    }
}
