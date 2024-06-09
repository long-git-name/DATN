using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AESCrypto : MonoBehaviour
{
    private static readonly string key = "u3SDVG3vk0RSLLUJ";
    private static readonly string iv = "0y4yEMBcxwPYJhQ7";

    public static string Encrypt(string plainText)
    {
        // Debug.Log(plainText);
        if (string.IsNullOrEmpty(plainText)) 
            throw new ArgumentNullException(nameof(plainText));

        // GenerateAESKeyAndIV(out byte[] generatedKey, out byte[] generatedIV);

        using (Aes aesAlg = Aes.Create())
        {
            // Debug.Log(aesAlg);
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            Debug.Log("AES Key: " + BitConverter.ToString(aesAlg.Key));
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);
            Debug.Log("AES IV: " + BitConverter.ToString(aesAlg.IV));

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            // Debug.Log(encryptor);

            using (var msEncrypt = new System.IO.MemoryStream())
            {
                // Debug.Log(msEncrypt);
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    // Debug.Log(csEncrypt);
                    using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                    {
                        // Debug.Log(swEncrypt);
                        swEncrypt.Write(plainText);
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) 
            throw new ArgumentNullException(nameof(cipherText));

        // GenerateAESKeyAndIV(out byte[] generatedKey, out byte[] generatedIV);
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new System.IO.MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {                    
                    using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                    {                        
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    private static RSACryptoServiceProvider rsa;

    // Hàm tạo khóa RSA
    public static void GenerateRSAKeys(out string publicKey, out string privateKey)
    {
        rsa = new RSACryptoServiceProvider(2048); // Tạo khóa RSA với độ dài 2048 bit
        publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
        privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
    }

    // private static readonly string publicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDxgNBASkKV8Zz6G0d9NBeRxhGnt6DVKmZlt+hx/7q2hhXzGDa/zSz5VU3ucyhsYn+Q9irY2wGBG4zCU3v9m5ULm4cWW8apUJZPmQfNRr7lMsj+kqDkfej3B8ScEoPTh9LUarLEbfX7LgCzs2yvK/8mrv85YIWsD7lLgzYsfb5S8wIDAQAB";
    // private static readonly string privateKey = "MIICXQIBAAKBgQDxgNBASkKV8Zz6G0d9NBeRxhGnt6DVKmZlt+hx/7q2hhXzGDa/zSz5VU3ucyhsYn+Q9irY2wGBG4zCU3v9m5ULm4cWW8apUJZPmQfNRr7lMsj+kqDkfej3B8ScEoPTh9LUarLEbfX7LgCzs2yvK/8mrv85YIWsD7lLgzYsfb5S8wIDAQABAoGAK3HRD1CMc2sHbRax+XtNzrGLVgjBbwZMhS3gPVjBJ1/d3I8dE7gc0lqS67HfQxk7gIFPTXXrBJFmevGtzHodf1tUZSot3i9PEk/W51Yp8xe21KqFGY8Eq6/iy5wWU5Upkfr7/5kiOT+tmClxVlljz5FmNDk/IJHsPL4cFE98FdECQQDxKTm8sQyNtNcWH+cW/3mzbl4zCez6zCHpn8dB3g7Aq3TZ5J6vGtK9VYRU9G5r15Oc5K5f1hOhZJATF/fz75/hAkEA+dZpnhbxJn7E4O3TEqgN5zgqQEmDHyKffw7yRw0/S1wny4j2/E6hXNkbOvn1H6eSZ+cdEwFARo7gS+yNw3R8VwJBAMTLyySZTrv8AZpDMeZrxwrj7wWeroroQ+o+Kj8T/Fm+tXjvvgIqz7qlsbxOkVTtpNt3D6V/DGb0RZd9YWlEwZ+j0CQQCo/vECYj0e3Zl/gFAj";

    // // Mã hóa dữ liệu với khóa công khai
    // public static string Encrypt(string plainText)
    // {
    //     // GenerateRSAKeys(out string publicKey, out string privateKey);
    //     using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
    //     {
    //         rsa.FromXmlString(publicKey);

    //         byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
    //         byte[] encryptedData = rsa.Encrypt(dataToEncrypt, false);
    //         return Convert.ToBase64String(encryptedData);
    //     }
    // }

    // // Giải mã dữ liệu với khóa riêng
    // public static string Decrypt(string cipherText)
    // {
    //     // GenerateRSAKeys(out string publicKey, out string privateKey);
    //     using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
    //     {
    //         rsa.FromXmlString(privateKey);

    //         byte[] dataToDecrypt = Convert.FromBase64String(cipherText);
    //         byte[] decryptedData = rsa.Decrypt(dataToDecrypt, false);
    //         return Encoding.UTF8.GetString(decryptedData);
    //     }
    // }

    public static void ExampleUsage()
    {
        // GenerateRSAKeys(out string publicKey, out string privateKey);
        // Debug.Log("Generated Key: " + generatedKey);
        // Debug.Log("Generated IV: " + generatedIV);
        string original = "Hello, world!";

        // Mã hóa dữ liệu
        string encrypted = Encrypt(original);
        Debug.Log("Encrypted: " + encrypted);

        // Giải mã dữ liệu
        string decrypted = Decrypt(encrypted);
        Debug.Log("Decrypted: " + decrypted);
    }

    public static void GenerateAESKeyAndIV(out byte[] generatedKey, out byte[] generatedIV)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.GenerateKey();
            aesAlg.GenerateIV();
            generatedKey = aesAlg.Key;
            generatedIV = aesAlg.IV;
        }
    }
}
