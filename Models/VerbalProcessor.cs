using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syncfusion.SpellChecker.Base
{
    internal class VerbalProcessor
    {
        #region Variables

        static string alphSimilarities = "aeiouyhw", betaSimilarities = "bfpv", gamaSimilarities = "cgjkqsxz";
        
        static string dtSimilarities = "dt", lSimilarities = "l", mnSimilarities = "mn", rSimilarities = "r";

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a list of phonetic suggestions for the given word in specified accuracy level.
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public static List<string> GetSimilarSounds(string errorWord, List<object> fileStream, AccuracyLevels accuracy)
        {
            string firstChar = errorWord[0].ToString();
            if (accuracy == AccuracyLevels.High)
            {
                firstChar = (alphSimilarities.Contains(firstChar)) ? alphSimilarities : firstChar;
                firstChar = (betaSimilarities.Contains(firstChar)) ? betaSimilarities : firstChar;
                firstChar = (gamaSimilarities.Contains(firstChar)) ? gamaSimilarities : firstChar;
                firstChar = (dtSimilarities.Contains(firstChar)) ? dtSimilarities : firstChar;
                firstChar = (mnSimilarities.Contains(firstChar)) ? mnSimilarities : firstChar;
            }
            var requirKey = PhoneValueWord(errorWord, accuracy);
            List<string> SimilarSounds = new List<string>();
            foreach (string line in fileStream)
            {
                if (firstChar.Contains(line[0].ToString()))
                {
                    var key = PhoneValueWord(line, accuracy);
                    if (requirKey == key)
                        SimilarSounds.Add(line);
                }
            }
            return SimilarSounds;
        }
       
        #endregion

        #region Helper Methods

        private static string PhoneValueWord(string str, AccuracyLevels accuracy)
        {
            var result = string.Empty;
            for (int i = 1; i < str.Length; i++)
            {
                var ch = str[i];
                if (alphSimilarities.Contains(ch.ToString()) && accuracy != AccuracyLevels.Low)
                    continue;
                result += PhoneValue(ch);
            }
            return result;
        }

        private static int PhoneValue(char ch)
        {
            if (betaSimilarities.Contains(ch.ToString()))
                return 1;
            if (gamaSimilarities.Contains(ch.ToString()))
                return 2;
            if (dtSimilarities.Contains(ch.ToString()))
                return 3;
            if (lSimilarities.Contains(ch.ToString()))
                return 4;
            if (mnSimilarities.Contains(ch.ToString()))
                return 5;
            if (rSimilarities.Contains(ch.ToString()))
                return 6;
            return 0;

        }     

        #endregion
    }
}
