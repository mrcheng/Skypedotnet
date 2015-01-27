namespace SkypeDotnet.Abstract
{
    public interface ISkypeLoginManager
    {
        string Login(LoginCredentials credentials);
    }
}