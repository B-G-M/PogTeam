using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

public class GFGEncryption
{
    public static string encodeString(String data){
     
          string answer = "";
          string publicKey = "PONG1234";
          string privateKey = "PONG4321";
          byte[] privateKeyBytes ={};
          privateKeyBytes = Encoding.UTF8.GetBytes(privateKey);
          byte[] publicKeyBytes = {};
          publicKeyBytes = Encoding.UTF8.GetBytes(publicKey);
          byte[] inputByteArray= System.Text.Encoding.UTF8.GetBytes(data);
          using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider())
          {
              var memoryStream = new MemoryStream();
              var cryptoStream = new CryptoStream(memoryStream, 
                  provider.CreateEncryptor(publicKeyBytes, privateKeyBytes),
                  CryptoStreamMode.Write);
              cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
              cryptoStream.FlushFinalBlock();
              answer = Convert.ToBase64String(memoryStream.ToArray());
          }
          return answer;
      }
     
      public static string decodeString(String data)
      { 
          string answer = "";
          string publicKey = "PONG1234";
          string privateKey = "PONG4321";
          byte[] privateKeyBytes ={};
          privateKeyBytes = Encoding.UTF8.GetBytes(privateKey);
          byte[] publicKeyBytes = {};
          publicKeyBytes = Encoding.UTF8.GetBytes(publicKey);
          byte[] inputByteArray= new byte[data.Replace(" ", "+").Length];
          inputByteArray = Convert.FromBase64String(data.Replace(" ", "+"));
          using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider())
          {
              var memoryStream = new MemoryStream();
              var cryptoStream = new CryptoStream(memoryStream,
                      provider.CreateDecryptor(publicKeyBytes, privateKeyBytes),
                  CryptoStreamMode.Write);
              cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
              cryptoStream.FlushFinalBlock();
              answer = Encoding.UTF8.GetString(memoryStream.ToArray());
          }
        return answer;
      }
}
