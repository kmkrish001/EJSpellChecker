using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Syncfusion.SpellChecker.Base
{
    internal class Sorter : IComparer<object>
    {
        CompareInfo _compareDetails = CultureInfo.InvariantCulture.CompareInfo;

        /// <summary>
        /// Compares the two objects.
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public virtual int Compare(object a, object b)
        {
            if (!string.IsNullOrEmpty(a.ToString()) && !string.IsNullOrEmpty(b.ToString()))
                return _compareDetails.Compare(a.ToString(), b.ToString());
            return a == null ? -1 : 1;
        }
        /// <summary>
        /// Compares the two String values
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public virtual int Compare(string word, string searchword)
        {
            if (!string.IsNullOrEmpty(word) && !string.IsNullOrEmpty(searchword))
            {
                return _compareDetails.Compare(word, searchword);
            }
            return word.CompareTo(searchword);
        }
    }

    internal class ReverseSorter : Sorter
    {
        /// <summary>
        /// Compares the two object values.
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public override int Compare(string word, string searchword)
        {
            return base.Compare(Reverse(word), Reverse(searchword));
        }
        /// <summary>
        /// Returns reverse String of input String
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public static string Reverse(string word)
        {
            char[] charArray = word.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }    
}
