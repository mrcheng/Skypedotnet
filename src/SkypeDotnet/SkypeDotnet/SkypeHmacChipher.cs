using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SkypeDotnet
{
    public class SkypeHmacChipher : ISkypeHmacChipher
    {
        public string Encrypt(string input, string lockAndKeyApp, string lockAndKeySecret )
        {
            //todo SHA256 encryption using lockAndKey appId and app secret. 
            //todo see skypeweb_hmac_sha256 function at ./skypeweb_src/skypeweb_util.c

            var sha = SHA256.Create();

            var encoding = Encoding.Default;
            var inputBytes = encoding.GetBytes(input);

            var lockAndKeySecretBytes = encoding.GetBytes(lockAndKeySecret);

            sha.TransformBlock(inputBytes, 0, inputBytes.Length, null, 0);
            sha.TransformFinalBlock(lockAndKeySecretBytes, 0, lockAndKeySecretBytes.Length);

            var shaBytes = sha.Hash;

            var shaIntegersOriginal = new uint[shaBytes.Length / 8];

            var shaIntegers = new uint[shaIntegersOriginal.Length];

            for (int i = 0; i < shaIntegersOriginal.Length; i++)
            {
                var arraySlise = shaBytes.Skip(i * 4).Take(4).ToArray();

                shaIntegersOriginal[i] = BitConverter.ToUInt32(arraySlise, 0);

                shaIntegers[i] = shaIntegersOriginal[i] & 0x7FFFFFFF;
            }


            //magic part
            var buffer = input + lockAndKeyApp;
            if (buffer.Length > 251)
            {
                buffer = buffer.Substring(0, 251);
            }
            if (buffer.Length % 8 != 0)
            {
                var fix = 8 - (buffer.Length % 8);
                buffer += new String('0', fix);
            }
            var byteBuffer = encoding.GetBytes(buffer);
            if (byteBuffer.Length % 4 != 0)
            {
                Array.Resize(ref byteBuffer, byteBuffer.Length + (4 - byteBuffer.Length % 4));
            }

            var chlStringParts = new uint[byteBuffer.Length / 4];

            for (var i = 0; i < byteBuffer.Length; i += 4)
            {
                chlStringParts[i / 4] = BitConverter.ToUInt32(byteBuffer, i);
            }
            UInt64 nHigh = 0, nLow = 0;
            for (var i = 0; i < chlStringParts.Length; i += 2)
            {
                var temp = (0x0E79A9C1 * (UInt64)chlStringParts[i]) % 0x7FFFFFFF;
                temp = (shaIntegers[0] * (temp + nLow) + shaIntegers[1]) % 0x7FFFFFFF;
                nHigh += temp;

                temp = (chlStringParts[i + 1] + temp) % 0x7FFFFFFF;
                nLow = (shaIntegers[2] * temp + shaIntegers[3]) % 0x7FFFFFFF;
                nHigh += nLow;
            }

            nLow = (nLow + shaIntegers[1]) % 0x7FFFFFFF;
            nHigh = (nHigh + shaIntegers[3]) % 0x7FFFFFFF;

            shaIntegersOriginal[0] ^= (uint)nLow;
            shaIntegersOriginal[1] ^= (uint)nHigh;
            shaIntegersOriginal[2] ^= (uint)nLow;
            shaIntegersOriginal[3] ^= (uint)nHigh;

            var newHash = shaIntegersOriginal;
            var result = new byte[newHash.Length * sizeof(uint)];

            Buffer.BlockCopy(newHash, 0, result, 0, result.Length);
            return BitConverter.ToString(result).Replace("-", "");
        }
    }
}