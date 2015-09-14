using System;

namespace NetSpell.SpellChecker.Dictionary.Phonetic {
    /// <summary>
    /// This class hold the settings for a phonetic rule
    /// </summary>
    public class PhoneticRule {
        private bool _BeginningOnly;
        private int[] _Condition = new int[256];
        private int _ConditionCount = 0;
        private int _ConsumeCount;
        private bool _EndOnly;
        private int _Priority;
        private bool _ReplaceMode = false;
        private string _ReplaceString;

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        public PhoneticRule() {
        }

        /// <summary>
        /// True if this rule should be applied to the beginning only
        /// </summary>
        public bool BeginningOnly {
            get { return this._BeginningOnly; }
            set { this._BeginningOnly = value; }
        }

        /// <summary>
        /// The ASCII condition array
        /// </summary>
        public int[] Condition {
            get { return this._Condition; }
        }

        /// <summary>
        /// The number of conditions
        /// </summary>
        public int ConditionCount {
            get { return this._ConditionCount; }
            set { this._ConditionCount = value; }
        }

        /// <summary>
        /// The number of chars to consume with this rule
        /// </summary>
        public int ConsumeCount {
            get { return this._ConsumeCount; }
            set { this._ConsumeCount = value; }
        }

        /// <summary>
        /// True if this rule should be applied to the end only
        /// </summary>
        public bool EndOnly {
            get { return this._EndOnly; }
            set { this._EndOnly = value; }
        }

        /// <summary>
        /// The priority of this rule
        /// </summary>
        public int Priority {
            get { return this._Priority; }
            set { this._Priority = value; }
        }

        /// <summary>
        /// True if this rule should run in replace mode
        /// </summary>
        public bool ReplaceMode {
            get { return this._ReplaceMode; }
            set { this._ReplaceMode = value; }
        }

        /// <summary>
        /// The string to use when replacing
        /// </summary>
        public string ReplaceString {
            get { return this._ReplaceString; }
            set { this._ReplaceString = value; }
        }
    }
}
