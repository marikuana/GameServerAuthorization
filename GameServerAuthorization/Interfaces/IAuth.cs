namespace GameServerAuthorization
{
    public interface IAuth
    {
        public IAccount? GetAccount(string username, string password);
    }
}
