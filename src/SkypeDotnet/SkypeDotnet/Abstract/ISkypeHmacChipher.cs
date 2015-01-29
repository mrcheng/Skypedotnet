namespace SkypeDotnet.Abstract
{
    public interface ISkypeHmacChipher
    {
        string Encrypt(string input, string lockAndKeyApp, string lockAndKeySecret);
    }
}