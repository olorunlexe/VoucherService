using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherService.Util
{
    public class CodeGenerator
    {
        /// <summary>
        /// Create a code with specific length and Character set
        /// </summary>
        /// <param name="length">the number of characters to be Generated e.g A code with length of 5 (12345)</param>
        /// <param name="characterSet">A pool of Characters to generate number from</param>
        /// <returns></returns>
        private static string GenerateCode(int length, string characterSet)
        {
            Random random = new Random();
            StringBuilder result;
            // Generate a random number
            result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characterSet[random.Next(characterSet.Length)]);
            }
            return result.ToString();
        }

        /// <summary>
        /// Add defined Prefix to generated code
        /// </summary>
        /// <param name="prefix">Given set of Strings to be Concatenated to the beginning of Generated Code</param>
        /// <param name="code">A system of characters, letters or symbols used for representation</param>
        /// <returns></returns>
        public static string GetCodeWithPrefix(string prefix, string code)
        {
            return prefix + code;
        }

        /// <summary>
        /// Add defined Suffix to generated code
        /// </summary>
        /// <param name="code">A system of characters, letters or symbols used for representation</param>
        /// <param name="suffix">Given set of Strings to be Concatenated at the end of Generated Code</param>
        /// <returns></returns>
        public static string GetCodeWithSuffix(string code, string suffix)
        {
            return code + suffix;

        }


    }
}
