using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syncfusion.SpellChecker.Base
{
    /// <summary>
    /// Represents a list of accuracy levels based on which phonetic matching is executed for filtering suggestions
    /// </summary>
    [ClassReference(IsReviewed = false)]
    public enum AccuracyLevels
    {
        /// <summary>
        /// Represents a high level which filters maximum possible suggestions.
        /// </summary>
        High,

        /// <summary>
        /// Represents a medium level which filters matching suggestions.
        /// </summary>
        Medium,

        /// <summary>
        /// Represents a most accurate level which filters accurate suggestions only.
        /// </summary>
        Low

    }
}
