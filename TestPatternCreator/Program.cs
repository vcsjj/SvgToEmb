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
            int angles = 2;
            int widths = 2;
            double minWidth = 0.4;
            double maxWidth = 0.8;

            double patchSize = 10; //mm
            double patchDistance = 4;

            double maxStepLength = 3;

            for (int i = 0; i < angles; i++)
            {
                double angle = i * 90.0 / angles;
                double xPosition = i * (patchSize + patchDistance);

                for (int j = 0; j < widths; j++) 
                {
                    double yPosition = j * (patchSize + patchDistance);
                    double lineHeight = minWidth +  (double)j * (maxWidth - minWidth) / (double)widths;
                    steps.AddRange(CreatePatch(patchSize, xPosition, yPosition, angle, lineHeight, maxStepLength));
                }
            }

            foreach (var item in steps)
            {
                Console.WriteLine(new CsvStepWriter(item).Write());
            }

            Console.WriteLine(CsvStepWriter.WriteClosingSequence());
        }

        static IEnumerable<Step> CreatePatch(double patchSize, double xPosition, double yPosition, double angle, double lineHeight, double maxStepLength)
        {
            var points = new MyPoint[] { new MyPoint(xPosition, yPosition), 
                new MyPoint(xPosition + patchSize, yPosition),
                new MyPoint(xPosition + patchSize, yPosition + patchSize),
                new MyPoint(xPosition, yPosition + patchSize)};
            
            Polygon p = new Polygon(points, null);

            var s = new AngleStepper(p, angle, lineHeight, maxStepLength);
            return s.CalculateSteps();
        }
    }
}
