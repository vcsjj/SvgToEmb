using System;
using System.Linq;
using ShapeLib;

namespace FileIO
{
    public class CsvStepWriter
    {
        private readonly Step step;

        public CsvStepWriter(Step s)
        {
            this.step = s;
        }

        public static string WriteClosingSequence() 
        {
            return ("#*#,#JUMP#,#0.0#,#0.0#").Replace('#', '"') + System.Environment.NewLine + ("#*#,#END#,#0.0#,#0.0#").Replace('#', '"');
        }

        public string Write()
        {
            string typestring;
            switch (this.step.Type)
            {
                case Step.StepType.Jump:
                    typestring = "TRIM";
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
            var culture = System.Globalization.CultureInfo.InvariantCulture;



            var parts = new string[] {
                "*",
                typestring,
                x.ToString("F4", culture),
                y.ToString("F4", culture)
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

