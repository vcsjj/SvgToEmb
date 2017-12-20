using NUnit.Framework;
using System;
using SvgToEmbCSV;

namespace SvgToEmbCSVTests
{
    [TestFixture()]
    public class OptionParserTests
    {
        [Test()]
        public void NoOptionEquivalentToCreateStiches()
        {
            string[] args = new string[] { };
            OptionParser op = new OptionParser(args);

            ActionType t = op.ParseForAction();
            Assert.AreEqual(ActionType.CreateStitches, t);
        }

        [Test()]
        public void StitchesOptionEquivalentToCreateStiches()
        {
            string[] args = new string[] { "-stitches" };
            OptionParser op = new OptionParser(args);

            ActionType t = op.ParseForAction();

            Assert.AreEqual(ActionType.CreateStitches, t);
        }

        [Test()]
        public void ColormapOptionEquivalentToCreateStiches()
        {
            string[] args = new string[] { "-colormap" };
            OptionParser op = new OptionParser(args);

            ActionType t = op.ParseForAction();

            Assert.AreEqual(ActionType.WriteColormap, t);
        }


        [Test()]
        public void FirstOptionWins()
        {
            string[] args = new string[] { "-colormap", "-stitches" };
            OptionParser op = new OptionParser(args);

            ActionType t = op.ParseForAction();

            Assert.AreEqual(ActionType.WriteColormap, t);
        }

        [Test()]
        public void InputFilenameAndOption()
        {
            string[] args = new string[] { "-colormap", "myFile" };
            OptionParser op = new OptionParser(args);

            string f = op.ParseForInputfile();

            Assert.AreEqual("myFile", f);
        }

        [Test()]
        public void OnlyInputFilename()
        {
            string[] args = new string[] { "myFile" };
            OptionParser op = new OptionParser(args);

            string f = op.ParseForInputfile();

            Assert.AreEqual("myFile", f);
        }
    }
}

