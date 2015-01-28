using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SkypeDotnet
{
    public interface ISkypeHmacChipher
    {
        string Encrypt(string input, string lockAndKeyApp, string lockAndKeySecret);
    }

    public class SkypeHmacChipher : ISkypeHmacChipher
    {
        public string Encrypt(string input, string lockAndKeyApp, string lockAndKeySecret )
        {
            //todo SHA256 encryption using lockAndKey appId and app secret. 
            //todo see skypeweb_hmac_sha256 function at ./skypeweb_src/skypeweb_util.c

            var sha = SHA256.Create();

            var encoding = Encoding.Default;
            var inputBytes = encoding.GetBytes(input);

            var lockAndKeyAppBytes = encoding.GetBytes(lockAndKeyApp);

            var lockAndKeySecretBytes = encoding.GetBytes(lockAndKeySecret);

            sha.TransformBlock(inputBytes, 0, inputBytes.Length, null, 0);
            sha.TransformFinalBlock(lockAndKeyAppBytes, 0, lockAndKeyAppBytes.Length);

            var shaBytes = sha.Hash;

            var shaIntegersOriginal = new uint[shaBytes.Length /4];

            var shaIntegers = new uint[shaIntegersOriginal.Length];

            for (int i = 0; i < shaIntegersOriginal.Length; i++)
            {
                var arraySlise = shaBytes.Skip(i * 4).Take(4).ToArray();

                Array.Reverse(arraySlise);
                shaIntegersOriginal[i] = BitConverter.ToUInt32(arraySlise, i * 4);

                shaIntegers[i] = shaIntegersOriginal[i] & 0x7FFFFFFF;
            }

            
            
            
            throw new NotImplementedException();

            
        }
    }
}