namespace SkypeDotnet.Abstract
{
    public interface ISkypeLoginManager
    {
        SkypeAuthParams Login(LoginCredentials credentials);
    }
}