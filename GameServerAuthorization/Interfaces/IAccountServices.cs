using GameServerData.Models;

namespace GameServerAuthorization
{
    public interface IAccountServices
    {
        public Account? GetAccount(string username, string password);
    }
}
