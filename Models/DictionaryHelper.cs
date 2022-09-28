using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Syncfusion.SpellChecker.Base
{
    internal class DictionaryHelper
    {
        #region Variables

        internal List<object> _wordCollection;

        #endregion
             
        #region Helper Methods

        internal bool ReadDictionary(Stream dictionaryStream)
        {
            if (dictionaryStream == null)
                return false;
            if(_wordCollection == null)
                _wordCollection = new List<object>();
            lock (this)
            {
                string tempstring = string.Empty;
                try
                {
                    StreamReader streamReader = new StreamReader(dictionaryStream, new UTF8Encoding());
                    while ((tempstring = streamReader.ReadLine()) != null)
                    {
                        _wordCollection.Add(tempstring);
                    }
                    streamReader.Dispose();                    
                }
                catch(Exception)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

    }
}
