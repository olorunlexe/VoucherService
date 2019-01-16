﻿using System;
using System.Collections.Generic;
using System.Text;

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
            // int separatorPosition = pattern.IndexOf(separator);
            var charsBeforeAfterSeparator = pattern.Split(separator, 2); //[##, ##] 
            var charsBeforeLength = charsBeforeAfterSeparator[0].Length;
            var charsAfterLength = charsBeforeAfterSeparator[1].Length;

            var beforeCode = GenerateCode(charsBeforeLength, characters);
            var afterCode = GenerateCode(charsBeforeLength, characters);

            return $"{beforeCode}{separator}{afterCode}";
            
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