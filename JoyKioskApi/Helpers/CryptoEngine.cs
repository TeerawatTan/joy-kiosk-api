using System.Security.Cryptography;
using System.Text;

namespace JoyKioskApi.Helpers
{
    public class CryptoEngine
    {
        private static string key = "b0cb368354084c48b41d43f2f08c950bCrystoEngine";

        public static string Encrypt(string input)
        {
            byte[] keyArray;
            SHA512CryptoServiceProvider hash = new SHA512CryptoServiceProvider();
            keyArray = hash.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            byte[] trimmedBytes = new byte[24];
            Buffer.BlockCopy(keyArray, 0, trimmedBytes, 0, 24);
            keyArray = trimmedBytes;

            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = keyArray;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string input)
        {
            byte[] keyArray;
            SHA512CryptoServiceProvider hash = new SHA512CryptoServiceProvider();
            keyArray = hash.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            byte[] trimmedBytes = new byte[24];
            Buffer.BlockCopy(keyArray, 0, trimmedBytes, 0, 24);
            keyArray = trimmedBytes;

            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = keyArray;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
