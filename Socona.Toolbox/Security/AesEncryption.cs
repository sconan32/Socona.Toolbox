using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Socona.ToolBox.Security
{
    public class AesEncryption
    {

        /// <summary>
        /// Aes加解密钥必须32位
        /// </summary>
        public static string AesKey
        {
            get
            {
                return "Ytiruces.Xobloot.Anocos"; // 此处可自定义，32个字符长度
            }
        }

        /// <summary>
        /// 获取Aes32位密钥
        /// </summary>
        /// <param name="key">Aes密钥字符串</param>
        /// <param name="encoding">编码类型</param>
        /// <returns>Aes32位密钥</returns>
        static byte[] GetAesKey(string key, Encoding encoding)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "Aes密钥不能为空");
            }
            if (key.Length < 32)
            {
                // 不足32补全
                key = key.PadRight(32, '0');
            }
            if (key.Length > 32)
            {
                key = key.Substring(0, 32);
            }
            return encoding.GetBytes(key);
        }

        /// <summary>
        /// Aes加密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptAes(string source)
        {
            return EncryptAes(source, AesKey);
        }

        /// <summary>
        /// Aes加密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="key">aes密钥，长度必须32位</param>
        /// <param name="model">运算模式</param>
        /// <param name="padding">填充模式</param>
        /// <param name="encoding">编码类型</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptAes(string source, string key, CipherMode model = CipherMode.ECB,
        PaddingMode padding = PaddingMode.PKCS7, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = GetAesKey(key, encoding);
                aesProvider.Mode = model;
                aesProvider.Padding = padding;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor())
                {
                    byte[] inputBuffers = encoding.GetBytes(source),
                        results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }

        /// <summary>
        /// Aes解密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptAes(string source)
        {
            return DecryptAes(source, AesKey);
        }

        /// <summary>
        /// Aes解密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="key">aes密钥，长度必须32位</param>
        /// <param name="model">运算模式</param>
        /// <param name="padding">填充模式</param>
        /// <param name="encoding">编码类型</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptAes(string source, string key, CipherMode model = CipherMode.ECB,
        PaddingMode padding = PaddingMode.PKCS7, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = GetAesKey(key, encoding);
                aesProvider.Mode = model;
                aesProvider.Padding = padding;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor())
                {
                    byte[] inputBuffers = Convert.FromBase64String(source);
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    return encoding.GetString(results);
                }
            }
        }

    }
}
