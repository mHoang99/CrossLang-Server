using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Threading.Tasks;

namespace CrossLang.Library
{
    /// <summary>
    /// Class chứa các hàm hash
    /// </summary>
    public static class CLHasher
    {
        /// <summary>
        /// Băm theo SHA256
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Hashed string</returns>
        public static string Sha256Hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        /// <summary>
        /// Hash theo BCrypt
        /// </summary>
        /// <param name="value"></param>
        /// <returns>String đã được hash</returns>
        public static string BcryptHash(string value)
        {
            return BCryptNet.HashPassword(value);
        }

        /// <summary>
        /// Kiểm tra bcrypt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedPassword"></param>
        /// <returns>true | false</returns>
        public static bool BCryptVerify(string password, string hashedPassword)
        {
            return BCryptNet.Verify(password, hashedPassword);
        }
    }
}
