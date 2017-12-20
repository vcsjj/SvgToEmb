namespace SvgToEmbCSV
{
    public enum ActionType {
        CreateStitches,
        WriteColormap,
    }

    public class OptionParser
    {
        private readonly string[] args;

        public OptionParser(string[] args)
        {
            this.args = args;
        }

        public string ParseForInputfile()
        {
            string inputFile = null;
            foreach (var arg in this.args)
            {
                if (!arg.StartsWith("-"))
                {
                    inputFile = arg;
                    break;
                }
            }

            return inputFile;
        }

        public ActionType ParseForAction()
        {
            ActionType t = ActionType.CreateStitches;
            foreach (var arg in this.args)
            {
                if (arg.ToLower() == "-colormap")
                {
                    t = ActionType.WriteColormap;
                    break;
                }
            }

            return t;
        }
    }
}

