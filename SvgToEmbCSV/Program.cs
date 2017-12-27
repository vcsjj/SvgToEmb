using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using ShapeLib;
using FileIO;

namespace SvgToEmbCSV
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            OptionParser op = new OptionParser(args);
            var t = op.ParseForAction();
            var f = op.ParseForInputfile();
            var cm = op.ParseForColormap();
            switch (t)
            {
                case ActionType.WriteColormap:
                    ShowColormap(f);
                    break;

                case ActionType.CreateStitches:
                    CreateStitches(f, cm);
                    break;
                default:
                    break;
            }

        }

        static void ShowColormap(string filename)
        {
            Console.WriteLine(ColorTranslation.Header);

            var r = XElement.Load(filename); 
            new SvgReader(r).Colors().ForEach(color => Console.WriteLine(new ColorTranslation {Color = color, StepAngle = 0.0, LineHeight = 0.5, MoveInside = 0.0, MaxStepLength = 3.0}));
        }

        static void CreateStitches(string filename, string colormap) 
        {
            var cmr = new ColorMapReader();

            var colortranslations = cmr.Read(TryReadLines(colormap));

            var culture = System.Globalization.CultureInfo.InvariantCulture;
            var r = XElement.Load(filename);
            var defaultMap = new List<ColorTranslation>
            {
                    new ColorTranslation{ Color = "#aaaa00", StepAngle = 90, LineHeight = 0.2 },
                    new ColorTranslation{ Color = "#aa0000", StepAngle =  0, LineHeight = 0.1 },
            };

            colortranslations = colortranslations.Count == 0 ? defaultMap : colortranslations;

            var polygonList = new SvgReader(r).Read();

            var steps = StepWriter.WriteStitches(colortranslations, polygonList);
            foreach (var step in steps)
            {
                Console.WriteLine(step);
            }
        }


        static IEnumerable<string> TryReadLines(string filename)
        {
            IEnumerable<string> enumerable;
            try
            {
                enumerable = System.IO.File.ReadLines(filename);
            }
            catch (Exception)
            {
                enumerable = new List<string>();
            }
            return enumerable;
        }

       
    }
}
