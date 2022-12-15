using GameServerData.Models;

namespace GameServerAuthorization
{
    public class Auth : IAccountServices
    {
        private List<Account> accounts = new List<Account>()
        {
            new Account() { Id = 1, Login = "Mar", Password = "12345" },
            new Account() { Id = 2, Login = "Marikuana", Password = "12345" },
        };

        public Account? GetAccount(string username, string password)
        {
            Account? account = accounts.Find(a => a.Login == username && a.Password == password);
            return account;
        }
    }
}