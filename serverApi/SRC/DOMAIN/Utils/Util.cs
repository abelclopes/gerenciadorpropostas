using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Konscious.Security.Cryptography;

namespace DOMAIN
{
    public static class Util
    {
        private const int Argon2Iterations = 3;
        private const int Argon2MemorySizeKb = 65536;
        private const int Argon2Parallelism = 2;
        private const int SaltSizeBytes = 16;
        private const int HashSizeBytes = 32;
        private const string Argon2Prefix = "$argon2id$";

        private static string GetPepper()
        {
            return Environment.GetEnvironmentVariable("PAPER_SECRETY") ?? string.Empty;
        }

        private static byte[] GetPasswordBytesWithPepper(string password)
        {
            var pepper = GetPepper();
            var merged = string.Concat(password, "::", pepper);
            return Encoding.UTF8.GetBytes(merged);
        }

        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Senha inválida para geração de hash");

            var salt = RandomNumberGenerator.GetBytes(SaltSizeBytes);

            var argon2 = new Argon2id(GetPasswordBytesWithPepper(password))
            {
                Salt = salt,
                Iterations = Argon2Iterations,
                MemorySize = Argon2MemorySizeKb,
                DegreeOfParallelism = Argon2Parallelism
            };

            var hash = argon2.GetBytes(HashSizeBytes);
            return $"{Argon2Prefix}v=19$m={Argon2MemorySizeKb},t={Argon2Iterations},p={Argon2Parallelism}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public static bool IsArgon2Hash(string hash)
        {
            return !string.IsNullOrWhiteSpace(hash) && hash.StartsWith(Argon2Prefix, StringComparison.Ordinal);
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedHash))
                return false;

            if (!IsArgon2Hash(storedHash))
                return false;

            var parts = storedHash.Split('$', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 5)
                return false;

            var configPart = parts[2];
            var config = configPart.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (config.Length != 3)
                return false;

            var memory = int.Parse(config[0].Replace("m=", string.Empty));
            var iterations = int.Parse(config[1].Replace("t=", string.Empty));
            var parallelism = int.Parse(config[2].Replace("p=", string.Empty));

            var salt = Convert.FromBase64String(parts[3]);
            var expectedHash = Convert.FromBase64String(parts[4]);

            var argon2 = new Argon2id(GetPasswordBytesWithPepper(password))
            {
                Salt = salt,
                Iterations = iterations,
                MemorySize = memory,
                DegreeOfParallelism = parallelism
            };

            var computedHash = argon2.GetBytes(expectedHash.Length);
            return CryptographicOperations.FixedTimeEquals(computedHash, expectedHash);
        }

        public static string GetSHA1HashData(string data)
        {
            //create new instance of md5
            SHA1 sha1 = SHA1.Create();

            //convert the input text to array of bytes
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();
        }
        public static bool ValidateSHA1HashData(string inputData, string storedHashData)
        {
            string getHashInputData = GetSHA1HashData(inputData);

            if (string.Compare(getHashInputData, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsLegacySha1Hash(string hash)
        {
            return !string.IsNullOrWhiteSpace(hash) && !IsArgon2Hash(hash);
        }

        public static DateTime convertDateTime(string date){
            string input = date;   
            string pattern = @"(-)|(/)";
            var datan = Regex.Split(input, pattern);
            
            return new DateTime(int.Parse(datan[4]), int.Parse(datan[2]),int.Parse(datan[0]));
            
        }
    }
}