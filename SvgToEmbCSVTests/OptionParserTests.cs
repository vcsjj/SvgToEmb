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
            string[] args = new string[] { "-writecolormap" };
            OptionParser op = new OptionParser(args);

            ActionType t = op.ParseForAction();

            Assert.AreEqual(ActionType.WriteColormap, t);
        }


        [Test()]
        public void FirstOptionWins()
        {
            string[] args = new string[] { "-writecolormap", "-stitches" };
            OptionParser op = new OptionParser(args);

            ActionType t = op.ParseForAction();

            Assert.AreEqual(ActionType.WriteColormap, t);
        }

        [Test()]
        public void InputFilenameAndOption()
        {
            string[] args = new string[] { "-writecolormap", "myFile" };
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

        [Test()]
        public void NoColormapAtAllFilename()
        {
            string[] args = new string[] { "--colormap" };
            OptionParser op = new OptionParser(args);

            string f = op.ParseForColormap();

            Assert.AreEqual(string.Empty, f);
        }

        [Test()]
        public void NoColormapSpecifiedFilename()
        {
            string[] args = new string[] { "--colormap" , "-stitches"};
            OptionParser op = new OptionParser(args);

            string f = op.ParseForColormap();

            Assert.AreEqual(string.Empty, f);
        }

        [Test()]
        public void ColormapSpecified()
        {
            string[] args = new string[] { "--colormap" , "map.csv"};
            OptionParser op = new OptionParser(args);

            string f = op.ParseForColormap();

            Assert.AreEqual("map.csv", f);
        }

        [Test()]
        public void ColormapAndInputFilenameSpecified()
        {
            string[] args = new string[] { "--colormap" , "map.csv", "in.svg"};
            OptionParser op = new OptionParser(args);

            string f = op.ParseForInputfile();

            Assert.AreEqual("in.svg", f);
        }

        [Test()]
        public void ColormapNameNotMistakenForInputFilename()
        {
            string[] args = new string[] { "--colormap" , "map.csv"};
            OptionParser op = new OptionParser(args);

            string f = op.ParseForInputfile();

            Assert.AreEqual(string.Empty, f);
        }
    }
}

