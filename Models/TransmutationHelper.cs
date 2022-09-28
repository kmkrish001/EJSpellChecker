using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syncfusion.SpellChecker.Base
{    
    internal class TransmutationHelper
    {
        static List<string> _combinations = new List<string>();

        private static void SwapIndexes(ref char x, ref char y)
        {
            if (x == y) 
                return;
            x ^= y;
            y ^= x;
            x ^= y;
        }

        /// <summary>
        /// Returns a list of possible combinations for the given character array
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public static List<string> GetTransmutations(char[] charArray)
        {
            _combinations.Clear();
            FindCombination(charArray, 0, charArray.Length - 1);
            return _combinations;
        }

        private static void FindCombination(char[] charArray, int startIndex, int endIndex)
        {
            if (startIndex != endIndex)
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    SwapIndexes(ref charArray[startIndex], ref charArray[i]);
                    FindCombination(charArray, startIndex + 1, endIndex);
                    SwapIndexes(ref charArray[startIndex], ref charArray[i]);
                }
            }
            else
            {
                _combinations.Add(new string(charArray));   
            }
        }
    }
}
