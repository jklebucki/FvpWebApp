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
            Console.WriteLine($"EncodingDecodingTest - Encrypt : {encrypted}");
            var decrypted = cryptography.Decrypt(encrypted);
            Console.WriteLine($"EncodingDecodingTest - Decrypt : {decrypted}");
            Assert.Equal(password, decrypted);
        }

        [Fact]
        public void EncodingDecodingEmptyPasswordTest()
        {
            Cryptography cryptography = new Cryptography();
            var password = string.Empty;
            var encrypted = cryptography.Encrypt(password);
            Console.WriteLine($"EncodingDecodingEmptyPasswordTest - Encrypt : {encrypted}");
            var decrypted = cryptography.Decrypt(encrypted);
            Console.WriteLine($"EncodingDecodingEmptyPasswordTest - Decrypt : {decrypted}");
            Assert.Equal(password, decrypted);
        }
    }
}
