using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomerLogin
{
    public class CryptoHelper
    {
        //crypto helper
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        public byte[] ComputeHash(string input, byte[] salt) {

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(input),
                salt,
                iterations,
                hashAlgorithm,
                keySize);

            return hash;
        }
        private byte[] IV =
       {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
            0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
        };

        public bool VerifyHash(string clearPassword,byte[] passwordHash, byte[] salt) {

            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(clearPassword, salt, iterations, hashAlgorithm, keySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, passwordHash);
        }

        public async Task<string> DecryptCreditCard(string clearPassword,  byte[] encryptedCreditCard)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword(clearPassword);
            aes.IV = IV;

            using MemoryStream input = new(encryptedCreditCard);
            using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);

            using MemoryStream output = new();
            await cryptoStream.CopyToAsync(output);

            return Encoding.Unicode.GetString(output.ToArray());
        }
        public async Task<byte[]> EncryptCreditCard(string clearPassword, string creditCard)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword(clearPassword);
            aes.IV = IV;

            using MemoryStream output = new();
            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);

            await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(creditCard));
            await cryptoStream.FlushFinalBlockAsync();

            return output.ToArray();
        }
        private byte[] DeriveKeyFromPassword(string password)
        {
            var emptySalt = Array.Empty<byte>();
            var iterations = 1000;
            var desiredKeyLength = 16; // 16 bytes equal 128 bits.
            var hashMethod = HashAlgorithmName.SHA384;
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
                                             emptySalt,
                                             iterations,
                                             hashMethod,
                                             desiredKeyLength);
        }

    }
}
