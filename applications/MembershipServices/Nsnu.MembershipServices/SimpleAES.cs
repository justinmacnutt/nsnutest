using System;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Web;

namespace Nsnu.MembershipServices
{
    public class SimpleAES
    {
        //Keep this key a secret
        private static byte[] _key = { 33, 161, 28, 208, 34, 107, 130, 45, 51, 184, 27, 162, 37, 112, 222, 7, 76, 155, 175, 144, 173, 53, 91, 29, 24, 234, 205, 13, 131, 236, 53, 85 };
        private UTF8Encoding _encoder;
        private RijndaelManaged _rm = new RijndaelManaged();


        public SimpleAES()
        {
           // RijndaelManaged _rm ;
            _rm.Key = _key;
            _encoder = new UTF8Encoding();
        }

        public string Encrypt(string unencrypted)
        {
            //Make sure we use a random salt
            _rm.GenerateIV();

            //Encrypt the plain text
            byte[] cipherTextBuffer = Encrypt(_encoder.GetBytes(unencrypted));

            //Create a buffer large enough to hold the cipher text and the salt and concatenate them together
            //The salt does not need to be private so we can store it in the db along as part of the encrypted password
            byte[] cipherSaltBuffer = new byte[cipherTextBuffer.Length + _rm.IV.Length];
            System.Buffer.BlockCopy(_rm.IV, 0, cipherSaltBuffer, 0, _rm.IV.Length);
            System.Buffer.BlockCopy(cipherTextBuffer, 0, cipherSaltBuffer, _rm.IV.Length, cipherTextBuffer.Length);

            return Convert.ToBase64String(cipherSaltBuffer);
        }

        public string Decrypt(string encrypted)
        {
            //The cipherblock includes the salt as the initial blocksize/8 bytes
            //Strip it out and use it to set the IV so we can decrypt properly
            int ivSize = _rm.BlockSize / 8;
            byte[] ivBuffer = new byte[ivSize];
            byte[] cipherSaltBuffer = Convert.FromBase64String(encrypted);
            System.Buffer.BlockCopy(cipherSaltBuffer, 0, ivBuffer, 0, ivSize);
            _rm.IV = ivBuffer;

            //Create a new byte[] to hold just the cipher text
            byte[] cipherTextBuffer = new byte[cipherSaltBuffer.Length - ivSize];
            System.Buffer.BlockCopy(cipherSaltBuffer, ivSize, cipherTextBuffer, 0, cipherSaltBuffer.Length - ivSize);

            //Now decrypt the cipher text with the correct salt
            return _encoder.GetString(Decrypt(cipherTextBuffer));
        }

        public string EncryptToUrl(string unencrypted)
        { 
            return HttpUtility.UrlEncode(Encrypt(unencrypted));
        }

        public string DecryptFromUrl(string encrypted)
        {
            return Decrypt(HttpUtility.UrlDecode(encrypted));
        }

        public byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, _rm.CreateEncryptor());
        }

        public byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, _rm.CreateDecryptor());
        }

        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            MemoryStream stream = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }
    }
}
