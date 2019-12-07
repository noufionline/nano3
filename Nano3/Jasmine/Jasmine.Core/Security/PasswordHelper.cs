using System;
using System.Linq;
using System.Security.Cryptography;

namespace Jasmine.Core.Security
{
    public class PasswordHelper
    {
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            //return buffer3 == buffer4;
            return ByteArraysEqual(buffer3, buffer4);

            //return true;
        }

        private static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            return !b1.Where((t, i) => t != b2[i]).Any();
        }

        public static string CreatePasswordHash(string newPassword)
        {
            const int saltSize = 16;
            const int bytesRequired = 32;
            byte[] array = new byte[1 + saltSize + bytesRequired];
            const int iterations = 1000; // 1000, afaik, which is the min recommended for Rfc2898DeriveBytes
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(newPassword, saltSize, iterations))
            {
                byte[] salt = pbkdf2.Salt;
                Buffer.BlockCopy(salt, 0, array, 1, saltSize);
                byte[] bytes = pbkdf2.GetBytes(bytesRequired);
                Buffer.BlockCopy(bytes, 0, array, saltSize + 1, bytesRequired);
            }
            return Convert.ToBase64String(array);
        }
    }
}
