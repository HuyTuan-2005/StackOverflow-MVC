using System;
using System.Security.Cryptography;
using System.Text;

namespace StackOverflow.Helpers
{
    public class SimpleHmacSha512
    {
        public static string ComputeRawKey(string plaintext, string rawSecret)
        {
            // rawSecret: key secret
            var key = Encoding.UTF8.GetBytes(rawSecret);
            using (var hmac = new HMACSHA512(key))
            {
                var data = Encoding.UTF8.GetBytes(plaintext);
                var mac = hmac.ComputeHash(data);
                return Convert.ToBase64String(mac);
            }
        }
    }
}