using System;
using System.Collections.Generic;
using System.Text;

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
        
        
        /// <summary>
        /// Creates a code specifying a pattern, characterset and a separator
        /// </summary>
        /// <param name="pattern">a sequence of # characters separated by a separator. e.g. ## - ###</param>
        /// <param name="characters">the set of numbers,alphabet or both(alphanumeric) character from which a code can be generated.
        /// Can only be specified using #
        /// </param>
        /// <param name="separator">a character that separates a pattern</param>
        /// <returns>code as string</returns>
        public static string GenerateCodeWithPattern(string pattern, string characters, string separator)
        {
            //##-##
            
            // int separatorPosition = pattern.IndexOf(separator);
            var charsBeforeAfterSeparator = pattern.Split(separator, 2); //[##, ##] 
            var charsBeforeLength = charsBeforeAfterSeparator[0].Length;
            var charsAfterLength = charsBeforeAfterSeparator[1].Length;

            var beforeCode = GenerateCode(charsBeforeLength, characters);
            var afterCode = GenerateCode(charsBeforeLength, characters);

            return $"{beforeCode}{separator}{afterCode}";
            
        }

        public static string GetCodeWithPrefix(string prefix, string code)
        {
            return prefix + code;
        }

        public static string GetCodeWithSuffix(string code, string suffix)
        {
            return code + suffix;
        }

    }
}
