using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherService.Util
{
    public class CodeGenerator
    {

        private static string GenerateCode(int length, string characters)
        {
            Random random = new Random();
            StringBuilder result;
            // Generate a random number
            result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        public static string GetCodeWithPrefix(string prefix, string code)
        {
            return prefix + code;
        }

        public static string GetCodeWithSuffix(string code, string suffix)
        {
            //string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return code + suffix;

        }


    }
}
