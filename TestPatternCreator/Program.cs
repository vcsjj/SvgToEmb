using System;
using System.Collections.Generic;
using ShapeLib;
using FileIO;

namespace TestPatternCreator
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            List<Step> steps = new List<Step>();
            int angles = 5;
            int widths = 5;
            double minWidth = 0.2;
            double maxWidth = 1.0;

            double patchSize = 10; //mm
            double patchDistance = 4;

            for (int i = 0; i < angles; i++)
            {
                double angle = i * 90.0 / angles;
                double xPosition = i * (patchSize + patchDistance);

                for (int j = 0; j < widths; j++) 
                {
                    double yPosition = j * (patchSize + patchDistance);
                    double lineHeight = minWidth +  (double)j * (maxWidth - minWidth) / (double)widths;
                    steps.AddRange(CreatePatch(patchSize, xPosition, yPosition, angle, lineHeight));
                }
            }

            foreach (var item in steps)
            {
                Console.WriteLine(new CsvStepWriter(item).Write());
            }

            Console.WriteLine(CsvStepWriter.WriteClosingSequence());
        }

        static IEnumerable<Step> CreatePatch(double patchSize, double xPosition, double yPosition, double angle, double lineHeight)
        {
            var points = new MyPoint[] { new MyPoint(xPosition, yPosition), 
                new MyPoint(xPosition + patchSize, yPosition),
                new MyPoint(xPosition + patchSize, yPosition + patchSize),
                new MyPoint(xPosition, yPosition + patchSize)};
            
            Polygon p = new Polygon(points, null);

            var s = new AngleStepper(p, angle);
            return s.CalculateSteps(lineHeight);
        }
    }
}
