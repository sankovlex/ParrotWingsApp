using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ParrotWings.Services.Secure
{
    public class DefaultPasswordProtector : IPasswordProtector
    {
        public (string hash, string salt) Protect(string password)
        {
            //generate random salt
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            //generate hash through HMACSHA1 algotihm
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));

            return (hashed, Convert.ToBase64String(salt));
        }

        public bool Verify(string password, string hash, string salt)
        {
            //generate hash through entered password and hash+salt in db
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                Convert.FromBase64String(salt),
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));

            return hash == hashed;
        }
    }
}
