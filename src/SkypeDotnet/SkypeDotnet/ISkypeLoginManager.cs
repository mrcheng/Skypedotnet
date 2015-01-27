namespace SkypeDotnet
{
    public interface ISkypeLoginManager
    {
        string Login(LoginCredentials credentials);
    }
}