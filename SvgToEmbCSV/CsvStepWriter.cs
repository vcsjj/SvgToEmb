using System;
using System.Linq;
using ShapeLib;

namespace SvgToEmbCSV
{
    public class CsvStepWriter
    {
        private readonly Step step;

        public CsvStepWriter(Step s)
        {
            this.step = s;
        }

        public string Write()
        {
            string typestring;
            switch (this.step.Type)
            {
                case Step.StepType.Jump:
                    typestring = "JUMP";
                    break;

                case Step.StepType.Stitch:
                default:
                    typestring = "STITCH";
                    break;
            }

            return this.CreateString(typestring, this.step.X, this.step.Y);
        }

        private string CreateString(string typestring, double x, double y)
        {
            var parts = new string[] {
                "*",
                typestring,
                x.ToString(),
                y.ToString()
            };

            var surroundedparts = parts.Select(p => this.SurroundWith(p, "\""));

            return string.Join(",", surroundedparts);
        }

        private string SurroundWith(string orig, string surrounder) 
        {
            return surrounder + orig + surrounder;
        }
    }
}

