using GameServerData;
using GameServerData.Models;

namespace GameServerAuthorization
{
    public class AccountService : IAccountServices
    {
        private Db _db;

        public AccountService(Db db)
        {
            _db = db;
        }

        public Account? GetAccount(string username, string password)
        {
            Account? account = _db.Accounts.FirstOrDefault(a => a.Login == username && a.Password == password);
            return account;
        }
    }
}