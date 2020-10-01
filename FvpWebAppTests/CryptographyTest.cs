using System;
using Xunit;
using FvpWebApp.Services;

namespace FvpWebAppTests
{
    public class CryptographyTest
    {
        [Fact]
        public void EncodingDecodingTest()
        {
            Cryptography cryptography = new Cryptography();

            var password = "password";
            var encrypted = cryptography.Encrypt(password);
            var decrypted = cryptography.Decrypt(encrypted);
            Assert.Equal(password, decrypted);
        }
    }
}
