using System;
using System.Text;
using FvpWebApp.Infrastructure;
using FvpWebApp.Services.Interfaces;

namespace FvpWebApp.Services
{
    public class Cryptography : ICryptography
    {
        public string Encrypt(string plainPassword)
        {
            if (!string.IsNullOrEmpty(plainPassword))
                return RandomValues.RandomString(48) + Convert.ToBase64String(Encoding.UTF8.GetBytes(
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(plainPassword))
                    ));
            else return string.Empty;
        }
        public string Decrypt(string encryptedPassword)
        {
            if (!string.IsNullOrEmpty(encryptedPassword))
                return Encoding.UTF8.GetString(Convert.FromBase64String(
                    Encoding.UTF8.GetString(Convert.FromBase64String(encryptedPassword.Substring(48)))
                    ));
            else return string.Empty;
        }
    }
}