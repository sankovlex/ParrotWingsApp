using System;
using System.Collections.Generic;
using System.Text;

namespace ParrotWings.Services.Secure
{
    public interface IPasswordProtector
    {
        bool Verify(string password, string hash, string salt);

        (string hash, string salt) Protect(string password);
    }
}
