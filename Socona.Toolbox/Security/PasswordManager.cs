using Socona.ToolBox.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.Security
{
    public class PasswordManager
    {
        public static async Task<string> GeneratePasswordFromFile(string fileName = null)
        {
            string result = string.Empty;
            await Task.Run(() =>
            {
                if (fileName == null)
                {
                    fileName = "\\password.txt";
                }
                var str = File.ReadAllText(fileName);
                result = AesEncryption.DecryptAes(str);
            });
            return result;
        }

        public static void SavePasswordToFile(string value, string fileName = null)
        {
            var result = AesEncryption.EncryptAes(value);

            if (fileName == null)
            {
                fileName = "\\password.txt";
            }
            File.WriteAllText(fileName, result);
        }


    }
}
