using GameServerData.Models;

namespace GameServerAuthorization.Interfaces
{
    public interface IAccountManager
    {
        Account? GetAccount(int accountId);
        Account? GetAccount(string username, string password);
        void UpdateRefreshToken(int accountId, string newRefreshToken, TimeSpan refreshTokenTime);
    }
}