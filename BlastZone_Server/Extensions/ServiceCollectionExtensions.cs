using LobbyServerShenkar.Interfaces;
using LobbyServerShenkar.Services;

namespace LobbyServerShenkar.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IRedisBaseService, RedisBaseService>()
                .AddSingleton<IPlayersRedisService, PlayersRedisService>()
                .AddSingleton<IXpRedisService, XpRedisService>()
                .AddSingleton<LevelService, LevelService>()
                ;
        }
    }
}
