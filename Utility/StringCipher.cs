using System.Security.Cryptography;
using System.Text;


namespace SpecFlowProject1.Utility;

public class StringCipher
{
    //Keysize of the encryption algorithm in bits
    private const int KeySize = 256;
    //Keysize of the encryption algorithm in bits
    private const int BlockSize = 128;

    //This constant determines the number of iterations for the password bytes generation function
    private const int DerivationIterations = 1000;
    private static byte[] GenerateRandomByteArray(int numBytes){
        var randomBytes = new byte[numBytes];
        RandomNumberGenerator.Create().GetBytes(randomBytes);
        return randomBytes;
    }

    public static string Encrypt(string plainText, string passPhrase)
    {
        //saltand Initialization Vector (IV) are randomly generated each time
        //Both are preprended to encrypt cipher text so that the same Salt IV values
        //can be used when decrypting
        var saltStringBytes=GenerateRandomByteArray(KeySize / 8);
        var ivStringBytes = GenerateRandomByteArray(BlockSize / 8);
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        using(var password= new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
        {
            var keyBytes = password.GetBytes(KeySize / 8);
            using(var symmetticKey = Aes.Create())
            {
                symmetticKey.BlockSize = BlockSize;
                symmetticKey.Mode = CipherMode.CBC;
                symmetticKey.Padding = PaddingMode.PKCS7;
                using (var encrytor = symmetticKey.CreateEncryptor(keyBytes, ivStringBytes))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encrytor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            //create the final bytes a concatenation of the random salt bytes the random iv bytes and the cipher bytes
                            var cipherTextBytes = saltStringBytes;
                            cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                            cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                            memoryStream.Close();
                            cryptoStream.Close();
                            return Convert.ToBase64String(cipherTextBytes);
                        }
                    }
                }
                
            }
        }
    }

    public static string Decrypt(string cipherText,string passPhrase)
    {
        //Get the complte stream of bytes that represent
        //[32 bytes of Salt] + [16 bytes of IV] + [n bytes of cipherText]
        var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);

        //get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes
        var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(KeySize / 8).ToArray();

        //Get the IV bytes by extracting the next 16 bytes from the supplied cipher text bytes
        var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(KeySize / 8).Take(BlockSize / 8).ToArray();

        //Get the actual cipher text bytes removing the first 48 bytes from the cipher text string
        var cipherTextBytes= cipherTextBytesWithSaltAndIv.Skip((KeySize+BlockSize)/8).Take(cipherTextBytesWithSaltAndIv.Length - ((KeySize+BlockSize) / 8)).ToArray();

        using (var password= new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
        {
            var keyBytes = password.GetBytes(KeySize / 8);
            using (var symmetricKey = Aes.Create())
            {
                symmetricKey.BlockSize = BlockSize;
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;
                using(var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                {
                    using(var memoryStream= new MemoryStream(cipherTextBytes))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        using (var streamReader = new StreamReader(cryptoStream, Encoding.UTF8))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

       
    }
}
