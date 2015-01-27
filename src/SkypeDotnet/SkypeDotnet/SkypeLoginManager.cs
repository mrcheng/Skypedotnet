using System;
using System.Security.Cryptography.X509Certificates;
using System.Web.UI.WebControls;

namespace SkypeDotnet
{
    public class SkypeLoginManager : ISkypeLoginManager
    {
        public string Login(LoginCredentials credentials)
        {
            throw new NotImplementedException();
        }
    }

    public interface ISkypeLoginManager
    {
        string Login(LoginCredentials credentials);
    }
}