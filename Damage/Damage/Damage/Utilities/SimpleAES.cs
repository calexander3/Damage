using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Damage.Utilities
{
    // ReSharper disable once InconsistentNaming
    public class SimpleAES
    {
        private readonly ICryptoTransform _encryptor;
        private readonly ICryptoTransform _decryptor;
        private readonly UTF8Encoding _encoder;

        public SimpleAES(byte[] encryptionKey, byte[] encryptionVector)
        {
            var rm = new RijndaelManaged();
            _encryptor = rm.CreateEncryptor(encryptionKey, encryptionVector);
            _decryptor = rm.CreateDecryptor(encryptionKey, encryptionVector);
            _encoder = new UTF8Encoding();
        }

        public byte[] EncryptString(string unencrypted)
        {
            return Encrypt(_encoder.GetBytes(unencrypted));
        }

        public string DecryptString(byte[] buffer)
        {
            return _encoder.GetString(Decrypt(buffer));
        }

        public byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, _encryptor);
        }

        public byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, _decryptor);
        }

        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            var stream = new MemoryStream();
            using (var cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }
    }

}