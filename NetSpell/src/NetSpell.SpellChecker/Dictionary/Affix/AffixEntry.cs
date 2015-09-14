
namespace NetSpell.SpellChecker.Dictionary.Affix {
    
    /// <summary>
    /// Rule Entry for expanding base words
    /// </summary>
    public class AffixEntry {
        private int _ConditionCount;
        private string _AddCharacters = string.Empty;
        private int[] _Condition = new int[256];
        private string _StripCharacters = string.Empty;

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        public AffixEntry() {
        }

        /// <summary>
        /// The characters to add to the string
        /// </summary>
        public string AddCharacters {
            get { return this._AddCharacters; }
            set { this._AddCharacters = value; }
        }

        /// <summary>
        /// The condition to be met in order to add characters
        /// </summary>
        public int[] Condition {
            get { return this._Condition; }
            set { this._Condition = value; }
        }

        /// <summary>
        /// The characters to remove before adding characters
        /// </summary>
        public string StripCharacters {
            get { return this._StripCharacters; }
            set { this._StripCharacters = value; }
        }

        /// <summary>
        /// The number of conditions that must be met
        /// </summary>
        public int ConditionCount {
            get { return this._ConditionCount; }
            set { this._ConditionCount = value; }
        }
    }
}
