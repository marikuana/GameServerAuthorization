using GameServerAuthorization;

public class Auth : IAuth
{
    private List<Account> accounts = new List<Account>()
    {
        new Account() { Id = 1, Name = "Mar", Password = "123" },
        new Account() { Id = 2, Name = "Marikuana", Password = "123" },
    };

    public IAccount? GetAccount(string username, string password)
    {
        Account? account = accounts.Find(a => a.Name == username && a.Password == password);
        return account;
    }
}
