namespace FvpWebApp.Services.Interfaces
{
    public interface ICryptography
    {
        string Encrypt(string plainPassword);
        string Decrypt(string encryptedPassword);
    }
}