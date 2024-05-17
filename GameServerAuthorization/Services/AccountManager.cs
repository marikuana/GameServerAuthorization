using GameServerAuthorization.Interfaces;
using GameServerData;
using GameServerData.Models;

namespace GameServerAuthorization.Services
{
    public class AccountManager : IAccountManager
    {
        private Db _db;

        public AccountManager(Db db)
        {
            _db = db;
        }

        public Account? GetAccount(string username, string password)
        {
            Account? account = _db.Accounts.FirstOrDefault(a => a.Login == username && a.Password == password);
            return account;
        }

        public Account? GetAccount(int accountId)
        {
            Account? account = _db.Accounts.FirstOrDefault(account => account.Id == accountId);
            return account;
        }

        public void UpdateRefreshToken(int accountId, string newRefreshToken, TimeSpan refreshTokenTime)
        {
            var account = GetAccount(accountId) ?? throw new Exception();
            account.RefreshToken = newRefreshToken;
            account.RefreshTokenExpiryTime = DateTime.UtcNow.Add(refreshTokenTime);
            _db.SaveChanges();
        }
    }
}
