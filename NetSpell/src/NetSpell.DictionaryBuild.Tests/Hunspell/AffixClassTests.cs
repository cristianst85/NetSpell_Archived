using NUnit.Framework;
using System;

namespace NetSpell.DictionaryBuild.Hunspell.Tests
{
    [TestFixture]
    public class AffixClassTests
    {
        [TestCase("PFX")]
        [TestCase("SFX")]
        [TestCase("pfx")]
        [TestCase("sfx")]
        public void FromString(string affixClassString)
        {
            var affixClass = AffixClass.FromString(affixClassString);
            Assert.AreEqual(affixClassString.ToUpper(), affixClass.ToString());
        }

        [TestCase("")]
        [TestCase("ABC")]
        [TestCase("abc")]
        public void FromStringThrowsArgumentException(string affixClassString)
        {
            Assert.Throws<ArgumentException>(() => AffixClass.FromString(affixClassString));
        }

        [TestCase(null)]
        public void FromStringThrowsArgumentNullException(string affixClassString)
        {
            Assert.Throws<ArgumentNullException>(() => AffixClass.FromString(affixClassString));
        }
    }
}
