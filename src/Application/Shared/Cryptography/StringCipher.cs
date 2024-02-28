using System.Security.Cryptography;

namespace Application.Shared.Cryptography;
public static class StringCipher
{
    // This constant is used to determine the keysize of the encryption algorithm in bits.
    // We divide this by 8 within the code below to get the equivalent number of bytes.
    private const int Keysize = 256;

    // This constant determines the number of iterations for the password bytes generation function.
    private const int DerivationIterations = 1000;

    private const string Password = "HLdgwb46udW3aI8qwWIeH2WIT2feNWFp";

    public static string Encrypt(string plaintext)
    {
        // Convert the plaintext string to a byte array
        byte[] plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plaintext);

        // Derive a new password using the PBKDF2 algorithm and a random salt
        var passwordBytes = new Rfc2898DeriveBytes(Password, Keysize, DerivationIterations, HashAlgorithmName.SHA256);

        // Use the password to encrypt the plaintext
        Aes encryptor = Aes.Create();
        encryptor.Key = passwordBytes.GetBytes(32);
        encryptor.IV = passwordBytes.GetBytes(16);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cs.Write(plaintextBytes, 0, plaintextBytes.Length);
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    public static string DecryptString(string encrypted)
    {
        // Convert the encrypted string to a byte array
        byte[] encryptedBytes = Convert.FromBase64String(encrypted);

        // Derive the password using the PBKDF2 algorithm
        var passwordBytes = new Rfc2898DeriveBytes(Password, Keysize, DerivationIterations, HashAlgorithmName.SHA256);

        // Use the password to decrypt the encrypted string
        Aes encryptor = Aes.Create();
        encryptor.Key = passwordBytes.GetBytes(32);
        encryptor.IV = passwordBytes.GetBytes(16);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        {
            cs.Write(encryptedBytes, 0, encryptedBytes.Length);
        }
        return System.Text.Encoding.UTF8.GetString(ms.ToArray());
    }
}