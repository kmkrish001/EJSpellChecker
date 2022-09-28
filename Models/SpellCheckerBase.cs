using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
#if SyncfusionLicense
using Syncfusion.Licensing;
#endif

namespace Syncfusion.SpellChecker.Base
{
    /// <summary>
    /// SpellChecker base helps to find erroneous spelling in a word and provides suggestions for it.
    /// </summary>
    [ClassReference(IsReviewed = false)]
    public class SpellCheckerBase : Object
    {
        #region Variables
        
        string[] _donotSuggest;

        string _emailRegEx = @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", _urlRegEx = @"(mailto\:|(news|(ht|f)tp(s?))\://)((\S+)|(\S+)( #([^#]+)#)?)",_filePathRegEx = @"[a-zA-Z0-9_$\-\.\\]*\\[a-zA-Z0-9_$\-\.\\]+";

        string[] _donotSuggestReversed = new string[] { "esra", "elohesra", "selohesra", "sesra", "daeh-ssa", "elohssa", "selohssa", "s'ssa", "dratsab", "hctib", "skcollob", "tihsllub", "detihsllub", "gnitihsllub", "stihsllub", "dettihsllub", "tilc", "stilc", "kcoc", "rekcuskcoc", "rekcus-kcoc", "srekcuskcoc", "srekcus-kcoc", "tnuc", "stnuc", "kcid", "daehkcid", "s'kcid", "odlid", "s'odlid", "sodlid", "traf", "detraf", "gnitraf", "straf", "s'traf", "kcuf", "lla-kcuf", "s'lla-kcuf", "dekcuf", "dekcuf", "pu-dekcuf", "s'pu-dekcuf", "rekcuf", "srekcuf", "s'rekcuf", "gnikcuf", "skcuf", "s'kcuf", "pukcuf", "rekcufrehtom", "rekcuf-rehtom", "srekcufrehtom", "srekcuf-rehtom", "reggin", "sreggin", "s'reggin", "seigro", "ygro", "s'ygro", "sinep", "s'sinep", "ssip", "dessip", "sessip", "gnissip", "tihs", "daehtihs", "sdaehtihs", "elohtihs", "stihs", "s'tihs", "dettihs", "gnittihs", "yttihs", "tuls", "stuls", "s'tuls", "cips", "scips", "s'cips", "tawt", "ecaftawt", "stawt", "rekcustawt", "anigav", "s'anigav", "knaw", "deknaw", "gniknaw", "sknaw", "erohw", "derohw", "moderohw", "serohw", "s'erohw" };

        Regex _emailExpression, _fileExpression, _urlExpression;

        Dictionary<string,List<string>> _filteredSuggestions;

        List<string> _phoneticWords;

        List<string> _Anagrams;

        DictionaryHelper _dictionary;        

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the instance of SpellChecker base.
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public SpellCheckerBase(Stream stream)
        {
            GetDontSuggestWords();
            GenerateRegularExpressions();
            _filteredSuggestions = new Dictionary<string, List<string>>();
            _Anagrams = new List<string>();
            _phoneticWords = new List<string>();
            _dictionary = new DictionaryHelper();
            AddWordsInDictionaryStream(stream);
        }

        /// <summary>
        /// Initializes the instance of SpellChecker base.
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public SpellCheckerBase()
        {
            GetDontSuggestWords();
            GenerateRegularExpressions();
            _filteredSuggestions = new Dictionary<string, List<string>>();
            _Anagrams = new List<string>();
            _phoneticWords = new List<string>();
            _dictionary = new DictionaryHelper();
#if SyncfusionLicense
            List<Platform> platforms = new List<Platform>();
            platforms.AddRange(new Platform[]
            {
                Platform.WindowsForms,
                Platform.WPF
            });
            FusionLicenseProvider.GetLicenseType(platforms);
#endif
        }

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Gets or Sets the boolean value to check AlphaNumericWords
        /// </summary>
        [ClassReference(IsReviewed = false)]

        public bool IgnoreAlphaNumericWords 
        { 
            get; 
           
            set; 
        }
       
        /// <summary>
        /// Gets or Sets the boolean value to check file names
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public bool IgnoreFileNames 
        { 
            get; 
           
            set; 
        }
      
        /// <summary>
        /// Gets or Sets the boolean value to check html tags
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public bool IgnoreHtmlTags 
        { 
            get;
            set; 
        }
       
        /// <summary>
        /// Gets or Sets the boolean value to check Email addresses
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public bool IgnoreEmailAddress 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or Sets the boolean value to check mixed case words
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public bool IgnoreMixedCaseWords 
        { 
            get; 
            set; 
        }
      
      
        /// <summary>
        /// Gets or Sets the boolean value to check upper case words
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public bool IgnoreUpperCaseWords 
        { 
            get;
            set; 
        }

      
        /// <summary>
        /// Gets or Sets the boolean value to check urls
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public bool IgnoreUrl 
        { 
            get; 
            set; 
        }

        
        #endregion
        
        #region PropertyChanged Callback Methods

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the word to dictionary word collection
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public bool AddWordToDictionary(string word)
        {
            bool isSuccess = false;
            if (_dictionary != null && _dictionary._wordCollection != null && !_dictionary._wordCollection.Contains(word))
            {
                _dictionary._wordCollection.Add(word);
                isSuccess = true;
            }
            return isSuccess;
        }

        /// <summary>
        /// Checks whether the word exists in dictionary
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public bool HasError(string word)
        {
            bool hasError = false;
            char[] separators = new char[] { ' ' };
            string[] words = word.Split(separators);
            words = words.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (_dictionary != null && _dictionary._wordCollection != null && _dictionary._wordCollection.Count > 0)
            {
                foreach (string errorword in words)
                    if (IsError(word))
                    {
                        hasError = true;
                        break;
                    }
            }
            return hasError;
        }

        /// <summary>
        /// Returns the suggestions for error word
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public Dictionary<string, List<string>> GetSuggestions(string word)
        {
            _filteredSuggestions = new Dictionary<string, List<string>>();
            if (_dictionary != null && _dictionary._wordCollection != null && _dictionary._wordCollection.Count > 0)
            {
                char[] separators = new char[] { ' ' };
                string[] words = word.Split(separators);
                words = words.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                foreach (string errorword in words)
                {
                    FilterSuggestions(errorword);
                }
            }
            return _filteredSuggestions;
        }

        /// <summary>
        /// Returns the anagrams for error word
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public List<string> GetAnagrams(string word)
        {
            _Anagrams = new List<string>();
            List<string> _casingAnagrams = new List<string>();
            if (_dictionary != null && _dictionary._wordCollection != null && _dictionary._wordCollection.Count > 0)
            {
                FilterAnagrams(word);
                return _Anagrams;
            }
            return new List<string>();
        }

        /// <summary>
        /// Returns the phonetic suggestions for error word in medium accuracy.
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public List<string> GetPhoneticWords(string word)
        {
            _phoneticWords = new List<string>();
            if (_dictionary != null && _dictionary._wordCollection != null && _dictionary._wordCollection.Count > 0)
            {
                FilterSuggestionByPhonetics(word.ToLower(),AccuracyLevels.Medium);
                return _phoneticWords;
            }
            return new List<string>();
        }

        /// <summary>
        /// Returns the phonetic suggestions for error word in specified accuracy.
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public List<string> GetPhoneticWords(string word, AccuracyLevels accuracy)
        {
            _phoneticWords = new List<string>();
            List<string> _casingPhonetics = new List<string>();
            if (_dictionary != null && _dictionary._wordCollection != null && _dictionary._wordCollection.Count > 0)
            {
                FilterSuggestionByPhonetics(word.ToLower(),accuracy);
                if(char.IsUpper(word[0]))
                {
                   for(int i=0;i<_phoneticWords.Count;i++)
                    {
                        string str = _phoneticWords[i].ToString();
                        str = str.Remove(0, 1);
                        str = str.Insert(0, char.ToUpper(_phoneticWords[i][0]).ToString());
                        _casingPhonetics.Add(str);
                    }
                   _phoneticWords = _casingPhonetics;
                }
                return _phoneticWords;
            }
            return new List<string>();
        }

        /// <summary>
        /// Returns whether the word matches Email or File name or URL pattern.
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public bool IsPatternMatch(string word)
        {
            return _emailExpression.IsMatch(word) || _fileExpression.IsMatch(word) || _urlExpression.IsMatch(word);
        }

        /// <summary>
        /// Adds the word from custom stream to dicitionary collection
        /// </summary>
        [ClassReference(IsReviewed = false)]
        public bool AddWordsInDictionaryStream(Stream stream)
        {
            if(_dictionary != null && stream != null)
                return _dictionary.ReadDictionary(stream);
            return false;
        }
        #endregion

        #region Helper Methods

        private void FilterSuggestions(string word)
        {
            FilterSuggestionByRelativeGrouping(word);            
            SortSuggestions();            
        }

        private void FilterAnagrams(string word)
        {
            if (word.Length <= 5)
                FilterSuggestionByAnagrams(word);            
        }

        private void FilterSuggestionByCompoundWords(string lookUpString)
        {
            string firstword = string.Empty, secondword= string.Empty;
            bool hasdictionaryentry = HasDictionaryEntry(lookUpString), iswordpresent = false;
            bool isfirstwordvalid = false,issecondwordvalid = false;
            for (int i = 0; i < lookUpString.Length; i++)
            {
                firstword += lookUpString[i];
                if (firstword.Length > 1 && (_dictionary._wordCollection.Contains(firstword) || _dictionary._wordCollection.Contains(firstword.ToLower())))
                    isfirstwordvalid = true;
                if (isfirstwordvalid)
                    break;
            }
            if (firstword.Length < lookUpString.Length)
            {
                secondword = lookUpString.ToLower().Substring(firstword.Length, lookUpString.Length - firstword.Length);
                if (secondword.Length > 1 && firstword.Length + secondword.Length == lookUpString.Length && (_dictionary._wordCollection.Contains(secondword) || _dictionary._wordCollection.Contains(secondword.ToLower())))
                    issecondwordvalid = true;  
            }
            if (isfirstwordvalid && issecondwordvalid)
            {
                iswordpresent = _filteredSuggestions[lookUpString].Contains(firstword + " " + secondword);
                if (!iswordpresent)
                {
                    if (!hasdictionaryentry)
                    { 
                        _filteredSuggestions.Add(lookUpString, new List<string>());
                    }
                    _filteredSuggestions[lookUpString].Add(firstword + " " + secondword);
                }
            }
        }

        private void FilterSuggestionByAnagrams(string lookUpString)
        {
            _Anagrams.Clear();
            var result = new List<string>();
            bool iswordpresent = false;
            foreach (string generatedstring in TransmutationHelper.GetTransmutations(lookUpString.ToLower().ToCharArray()))
            {
                iswordpresent = result.Contains(generatedstring);
                if (!_donotSuggest.Contains(generatedstring.ToLower()) && generatedstring.ToLower() != lookUpString.ToLower() && !iswordpresent && !_donotSuggest.Contains(generatedstring))
                {
                    if (_dictionary._wordCollection.Contains(generatedstring) || _dictionary._wordCollection.Contains(generatedstring.ToLower()))
                    {
                        result.Add(generatedstring);
                    }
                }
            }
            _Anagrams = result;
        }

        private void FilterSuggestionByPhonetics(string lookUpString, AccuracyLevels accuracy)
        {
            bool hasdictionaryentry = HasDictionaryEntry(lookUpString);
            if (lookUpString.Length > 0)
            _phoneticWords = VerbalProcessor.GetSimilarSounds(lookUpString.ToLower(), _dictionary._wordCollection, accuracy);
            SortPhoneticWords(lookUpString);
        }

        private void FilterSuggestionByRelativeGrouping(string lookUpString)
        {
            bool hasdictionaryentry = HasDictionaryEntry(lookUpString);
            List<string> binarysearchlist, startswithlist, refinedresult;
            if (lookUpString.Length <= 3)
            {
                binarysearchlist = new List<string>();
              foreach(string word in _dictionary._wordCollection)
                  if(word.Length > 2  && word.Length <= 5 && word.StartsWith(lookUpString[0].ToString()))
                      binarysearchlist.Add(word);
            }
            else
            {
                binarysearchlist = FilterSuggestionsBySearch(lookUpString.ToLower());
            }
            startswithlist = new List<string>();
            refinedresult = new List<string>();
            bool hasValidSubStringAtStart = false;
            for (int i = 3; i <= lookUpString.Length; i++)
            {
                string searchstring = lookUpString.Substring(0, i);
                if (_dictionary._wordCollection.Contains(searchstring))
                    hasValidSubStringAtStart = true;
                if (hasValidSubStringAtStart)
                    break;
            }
            for (int i = lookUpString.Length; i >= 1; i--)
            {
                string searchword = lookUpString.Substring(0, i);
                if(!HasError(searchword.ToLower()))
                {
                    foreach (string word in binarysearchlist)
                    {
                        if (word.StartsWith(searchword.ToLower()))
                        {
                            startswithlist.Add(word);
                        }
                    } 
                }
            }
            if (!hasValidSubStringAtStart)
            {
                for (int i = 2; i <= lookUpString.Length; i++)
                {
                    string searchstring = lookUpString.Substring(0, i);
                    foreach (string word in _dictionary._wordCollection)
                    {
                        if (word.StartsWith(searchstring.ToLower()))
                        {
                            startswithlist.Add(word);
                        }
                    }
                }
            }
            for (int i = 0; i < startswithlist.Count; i++)
            {
                string word = startswithlist[i];
                bool iswordpresent = false;
                int threshhold = 700;
                if (lookUpString.Length < 4)
                    threshhold = 600;            
                if (!hasdictionaryentry)
                    iswordpresent = refinedresult.Contains(word);
                else
                    iswordpresent = _filteredSuggestions[lookUpString].Contains(word);
                if (!iswordpresent && CanSuggest(word, lookUpString.ToLower(), threshhold))
                {
                    if (hasdictionaryentry)
                        _filteredSuggestions[lookUpString].Add(word);
                    else
                    {
                        string str = word.Remove(0, 1);
                        str = str.Insert(0, lookUpString[0].ToString());
                        refinedresult.Add(str);
                    }
                }
            }
            if (!hasdictionaryentry)
                _filteredSuggestions.Add(lookUpString, refinedresult);            
        }

        private bool CanSuggest(string word, string lookUpString, int threshhold)
        {
            return CheckWordLength(word, lookUpString) && CheckRepetionInWord(word, lookUpString) && CheckRepetionInLookUpString(word, lookUpString, threshhold);
        }

        private bool CheckWordLength(string word, string lookupString)
        {
            // Checks whether suggested word's length is not greater than 1.5 length and lesser than 0.5 length.
            if ((word.Length >= lookupString.Length && ((word.Length - lookupString.Length) > (lookupString.Length * 0.5))) || ((lookupString.Length - word.Length) > (lookupString.Length * 0.5)))
                return false;
            return true;
        }

        private bool CheckRepetionInWord(string word, string lookupString)
        {
            int _characterRepetitionCount;
            double _repetitionoffset = 0.38;
            // Checks whether suggested word's length is not greater than 1.5 length and lesser than 0.5 length.
            if ((word.Length >= lookupString.Length && ((word.Length - lookupString.Length) > (lookupString.Length * 0.5))) || ((lookupString.Length - word.Length) > (lookupString.Length * 0.5)))
                return false;
            _characterRepetitionCount = 0;
            for (int i = 0; i < lookupString.Length; i++)
                if (word.IndexOf(lookupString.ToLower()[i]) > -1)
                    _characterRepetitionCount++;
            //Checks whether suggested word has maximum characters that are available in look up string
            if (_characterRepetitionCount < (lookupString.Length - (lookupString.Length * _repetitionoffset)))
                return false;
            return true;
        }

        private bool CheckRepetionInLookUpString(string word, string lookupString, int threshhold)
        {            
            return GetRepetitionOffset(word,lookupString) >= threshhold;
        }

        internal int GetRepetitionOffset(string word, string lookupString)
        {
            int _offset = 0, _caretIndex = 0, _currentCharIndex = 0, _minLength;
            bool _isChecked;
            int[] _checkedIndexes = new int[word.Length];
            for (int i = 0; i < word.Length; i++)
            {
                _minLength = word.Length;
                for (int j = 0; (j < lookupString.Length) && (_minLength != 0); j++)
                {
                    _isChecked = false;
                    if ((word[i] == lookupString.ToLower()[j]) || (string.Compare(word, i, lookupString, j, 1, StringComparison.OrdinalIgnoreCase) == 0))
                    {
                        for (int k = 0; k < _caretIndex; k++)
                            if (_checkedIndexes[k] == j)
                                _isChecked = true;
                        if (!_isChecked)
                        {
                            if ((i - j > 0 ? i - j : j - i) < _minLength)
                            {
                                _minLength = i - j > 0 ? i - j : j - i;
                                _currentCharIndex = j;
                            }
                        }
                    }
                }
                _minLength = _minLength * _minLength > word.Length ? word.Length : _minLength * _minLength;
                if (_minLength != word.Length)
                {
                    _checkedIndexes[_caretIndex] = _currentCharIndex;
                    if (_caretIndex < 15)
                        _caretIndex++;
                    _offset += (1000 * (word.Length - _minLength)) / word.Length;
                }
            }
            return (((_offset / lookupString.Length) + (_offset / word.Length)) / 2);
        }

        private List<string> FilterSuggestionsBySearch(string word)
        {
            List<object> list = new List<object>();
            int sortedindex, reversesortedindex, _minListCount = 100, _maxListCount = _minListCount * 2;
            if (_dictionary._wordCollection.Count < _minListCount)
                _minListCount = 0;
            if (_dictionary._wordCollection.Count < _maxListCount)
                _maxListCount = _dictionary._wordCollection.Count;
            // Pick 200 items above and below the perfectly matched word
            sortedindex = (sortedindex = _dictionary._wordCollection.BinarySearch(word, new Sorter())) < 0 ? sortedindex *= -1 : sortedindex;
            sortedindex = sortedindex > (_dictionary._wordCollection.Count - _minListCount) ? _dictionary._wordCollection.Count - _minListCount : (sortedindex < _minListCount ? _minListCount : sortedindex);
           if (_dictionary._wordCollection.Count > _maxListCount)
            {
                if (sortedindex - _minListCount >= 0)
                    list.AddRange(_dictionary._wordCollection.GetRange(sortedindex - _minListCount, _maxListCount));
                // Pick 200 items above and below the perfectly matched reversed word
                reversesortedindex = (reversesortedindex = _dictionary._wordCollection.BinarySearch(word, new ReverseSorter())) < 0 ? reversesortedindex *= -1 : reversesortedindex;
                reversesortedindex = reversesortedindex < _minListCount ? _minListCount : ((reversesortedindex > (_dictionary._wordCollection.Count - _minListCount)) ? _dictionary._wordCollection.Count - _minListCount : reversesortedindex);
                if (reversesortedindex - _minListCount >= 0)
                    list.AddRange(_dictionary._wordCollection.GetRange(reversesortedindex - _minListCount, _maxListCount));
            }
            else
            {
                list.AddRange(_dictionary._wordCollection.GetRange(_minListCount, _maxListCount));
            }
            return list.Cast<string>().ToList();
        }

        private void SortSuggestions()
        {
            Dictionary<string, int> suggestionorders = new Dictionary<string, int>();
            foreach (KeyValuePair<string, List<string>> pair in _filteredSuggestions)
            {
                suggestionorders.Clear();
                for (int i = 0; i < pair.Value.Count; i++)
                    if(!suggestionorders.ContainsKey(pair.Value[i]))
                        suggestionorders.Add(pair.Value[i], Compare(pair.Key.ToLower(), pair.Value[i]));
                pair.Value.Clear();
                foreach (var item in suggestionorders.OrderBy(key => key.Value))
                {
                    if(!_donotSuggest.Contains(item.Key))
                        pair.Value.Add(item.Key);
                }
            }
        }

        private void SortPhoneticWords(string errorword)
        {
            Dictionary<string, int> suggestionorders = new Dictionary<string, int>();
            for (int i = 0; i < _phoneticWords.Count; i++)
            {
                suggestionorders.Add(_phoneticWords[i],Compare(errorword, _phoneticWords[i]));
            }
            _phoneticWords.Clear();
            foreach (var item in suggestionorders.OrderBy(key => key.Value))
            {
                if (!_donotSuggest.Contains(item.Key))
                    _phoneticWords.Add(item.Key);
            }
        }

        private bool HasDictionaryEntry(string key)
        {
            return _filteredSuggestions.ContainsKey(key);
        }

        private void GetDontSuggestWords()
        {
            _donotSuggest = new string[_donotSuggestReversed.Length];
            for (int i = 0; i < _donotSuggestReversed.Length; i++)
            {
                char[] charArray = _donotSuggestReversed[i].ToCharArray();
                Array.Reverse(charArray);
                _donotSuggest[i] = new string(charArray);
            }
        }

        private bool IsNumeric(string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (!char.IsDigit(word[i]))
                    return false;
            }
            return true;
        }

        private bool IsAllUpperCase(string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsLower(word[i]) || char.IsDigit(word[i]))
                    return false;
            }
            return true;
        }

        private bool IsMixedCase(string word)
        {
            bool flag1 = false, flag2 = false;
            int startindex = char.IsUpper(word[0]) ? 1 : 0;
            for (int i = startindex; i < word.Length; i++)
            {
                if (char.IsLower(word[i]))
                {
                    flag2 = true;
                    if (flag1)
                        return true;
                }
                else if (char.IsUpper(word[i]))
                {
                    flag1 = true;
                    if (flag2)
                        return true;
                }
            }
            return false;
        }

        private bool IsAlphaNumericWord(string word)
        {
            bool hasNumber = false, hasAlphabet = false;
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsNumber(word[i]))
                {
                    hasNumber = true;
                    if (hasAlphabet)
                        return true;
                }
                else if (char.IsLetter(word[i]))
                {
                    hasAlphabet = true;
                    if (hasNumber)
                        return true;
                }
            }
            return false;
        }

        private int Compare(string suggestion, string word)
        {
            int suggestionlength = suggestion.Length;
            int wordlength = word.Length;
            int[,] comparisionmatrix = new int[suggestionlength + 1, wordlength + 1];
            for (int i = 1; i <= suggestionlength; i++)
            {
                for (int j = 1; j <= wordlength; j++)
                {
                    int cost = (word[j - 1] == suggestion[i - 1]) ? 0 : 1;
                    comparisionmatrix[i, j] = Math.Min(Math.Min(comparisionmatrix[i - 1, j] + 1, comparisionmatrix[i, j - 1] + 1), comparisionmatrix[i - 1, j - 1] + cost);
                }
            }
            return comparisionmatrix[suggestionlength, wordlength];
        }

        private bool IsError(string word)
        {
            
            if (_dictionary._wordCollection.Contains(word))
                return false;
            if (IsAlphaNumericWord(word) && !IsURL(word))
                if (!IgnoreAlphaNumericWords)
                    return true;
                else
                    return false;
            if (IsHTMLTag(word))
                if (!IgnoreHtmlTags)
                    return true;
                else
                    return false;
            if (IsEmailAddress(word))
                if (!IgnoreEmailAddress)
                    return true;
                else
                    return false;
            if (IsURL(word))
                if (!IgnoreUrl)
                    return true;
                else
                    return false;
            if (IsFilePath(word))
                if (!IgnoreFileNames)
                    return true;
                else
                    return false;
            if (IsMixedCase(word))
                if (!IgnoreMixedCaseWords)
                    return true;
                else
                    return false;
            if (IsNumeric(word))
                   return false;
            
            if (IsAllUpperCase(word))
                if (!IgnoreUpperCaseWords)
                    return true;
                else
                    return false;
            if (_dictionary._wordCollection.Contains(word) || _dictionary._wordCollection.Contains(word.ToLower()))
                return false;
            else
                return true;
        }        

        private bool IsEmailAddress(string word)
        {
            return _emailExpression.IsMatch(word);
        }

        private bool IsFilePath(string word)
        {
            return _fileExpression.IsMatch(word);
        }

        private bool IsURL(string word)
        {
            return _urlExpression.IsMatch(word);
        }

        private bool IsHTMLTag(string word)
        {
            if (word.StartsWith("<") && word.EndsWith(">"))
                return true;
            return false;
        }

        private void GenerateRegularExpressions()
        {
            _emailExpression = new Regex(_emailRegEx, RegexOptions.None);
            _fileExpression = new Regex(_filePathRegEx, RegexOptions.None);
            _urlExpression = new Regex(_urlRegEx, RegexOptions.None);
        }

        #endregion
    }
}
