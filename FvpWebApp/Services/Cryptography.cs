using System;
using System.Text;
using FvpWebApp.Services.Interfaces;

namespace FvpWebApp.Services
{
    public class Cryptography : ICryptography
    {
        public string Decrypt(string encryptedPassword)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(
                Encoding.UTF8.GetString(Convert.FromBase64String(encryptedPassword))
                ));
        }

        public string Encrypt(string plainPassword)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(
                Convert.ToBase64String(Encoding.UTF8.GetBytes(plainPassword))
                ));
        }
    }
}