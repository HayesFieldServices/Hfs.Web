using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace Hfs.Web.RestApi
{        
    public struct HashedPassword
    {
        public int Iterations;
        public string Salt;
        public string Hash;
    }

    /// <summary>
    /// Salted password hashing with PBKDF2-SHA1.
    /// </summary>
    public class PasswordHash
    {
        // Password Hashing Constants
        public const int SaltByteSize = 32;
        public const int HashByteSize = 32;
        public const int PBKDF2Iterations = 1000000;

        /// <summary>
        /// Creates a salted PBKDF2 hash of a password
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <returns>An instance of HashedPassword</returns>
        private static HashedPassword CreateHash(string password)
        {
            // Generate a random salt
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];
            csprng.GetBytes(salt);

            // Hash the password and encode the parameters
            byte[] hash = PBKDF2(password, salt, PBKDF2Iterations, HashByteSize);

            return new HashedPassword
            {
                Iterations = PBKDF2Iterations,
                Salt = Convert.ToBase64String(salt),
                Hash = Convert.ToBase64String(hash)
            };
        }

        /// <summary>
        /// Computes the PBKDF2-SHA1 hash of a password.
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <param name="salt">The salt</param>
        /// <param name="iterations">The PBKDF2 iteration count</param>
        /// <param name="outputBytes">The length of the hash to generate, in bytes</param>
        /// <returns>A hash of the password</returns>
        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}