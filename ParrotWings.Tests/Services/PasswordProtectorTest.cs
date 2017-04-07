using ParrotWings.Services.Secure;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ParrotWings.Tests.Services
{
    public class PasswordProtectorTest
    {
        private IPasswordProtector passwordProtector;

        public PasswordProtectorTest()
        {
            this.passwordProtector = new DefaultPasswordProtector();
        }

        [Fact]
        public void Verify_Entered_Password()
        {
            //Arrage
            string password = "1234A5b6@";

            //Act
            var protect = passwordProtector.Protect(password);

            //Assert
            Assert.True(passwordProtector.Verify(password, protect.hash, protect.salt));
        }
    }
}
