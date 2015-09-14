using System.Collections;

namespace NetSpell.SpellChecker.Dictionary.Affix {
    /// <summary>
    /// A strongly typed enumerator for 'AffixRuleCollection'
    /// </summary>
    public class AffixRuleEnumerator : IDictionaryEnumerator {
        private IDictionaryEnumerator innerEnumerator;

        internal AffixRuleEnumerator(AffixRuleCollection enumerable) {
            this.innerEnumerator = enumerable.InnerHash.GetEnumerator();
        }

        #region Implementation of IDictionaryEnumerator

        /// <summary>
        ///  gets the key of the current AffixRuleCollection entry.
        /// </summary>
        public string Key {
            get {
                return (string)this.innerEnumerator.Key;
            }
        }

        object IDictionaryEnumerator.Key {
            get {
                return this.Key;
            }
        }

        /// <summary>
        /// gets the value of the current AffixRuleCollection entry.
        /// </summary>
        public AffixRule Value {
            get {
                return (AffixRule)this.innerEnumerator.Value;
            }
        }

        object IDictionaryEnumerator.Value {
            get {
                return this.Value;
            }
        }

        /// <summary>
        ///  gets both the key and the value of the current AffixRuleCollection entry.
        /// </summary>
        public System.Collections.DictionaryEntry Entry {
            get {
                return this.innerEnumerator.Entry;
            }
        }

        #endregion

        #region Implementation of IEnumerator

        /// <summary>
        /// Sets the enumerator to the first element in the collection
        /// </summary>
        public void Reset() {
            this.innerEnumerator.Reset();
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection
        /// </summary>
        public bool MoveNext() {
            return this.innerEnumerator.MoveNext();
        }

        /// <summary>
        /// Gets the current element from the collection
        /// </summary>
        public object Current {
            get {
                return this.innerEnumerator.Current;
            }
        }
        #endregion
    }
}
