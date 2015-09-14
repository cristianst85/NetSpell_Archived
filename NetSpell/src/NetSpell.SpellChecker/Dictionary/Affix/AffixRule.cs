using System;

namespace NetSpell.SpellChecker.Dictionary.Affix {
    /// <summary>
    /// Rule for expanding base words
    /// </summary>
    public class AffixRule {
        private bool _AllowCombine = false;
        private AffixEntryCollection _AffixEntries = new AffixEntryCollection();
        private string _Name = string.Empty;

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        public AffixRule() {
        }

        /// <summary>
        /// Allow combining prefix and suffix
        /// </summary>
        public bool AllowCombine {
            get { return this._AllowCombine; }
            set { this._AllowCombine = value; }
        }

        /// <summary>
        /// Collection of text entries that make up this rule
        /// </summary>
        public AffixEntryCollection AffixEntries {
            get { return this._AffixEntries; }
            set { this._AffixEntries = value; }
        }

        /// <summary>
        /// Name of the Affix rule
        /// </summary>
        public string Name {
            get { return this._Name; }
            set { this._Name = value; }
        }
    }
}
