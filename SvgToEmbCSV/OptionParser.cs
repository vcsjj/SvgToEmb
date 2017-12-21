using System.Linq;
using System;

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
            string candidate =  this.args
                .Where(arg => !arg.StartsWith("-"))
                .DefaultIfEmpty(string.Empty)
                .Last();

            int index = Array.IndexOf(this.args, candidate);
            if (index >= 1 && this.args[index - 1].StartsWith("--"))
            {
                return string.Empty;
            }
            else
            {
                return candidate;
            }
        }

        public ActionType ParseForAction()
        {
            ActionType t = ActionType.CreateStitches;
            foreach (var arg in this.args)
            {
                if (arg.ToLower() == "-writecolormap")
                {
                    t = ActionType.WriteColormap;
                    break;
                }
            }

            return t;
        }

        public string ParseForColormap()
        {
            string cm = string.Empty;
            for(int i = 0; i< args.Length -1; i++)
            {
                if (args[i].ToLower() == "--colormap")
                {
                    if (!args[i + 1].StartsWith("-"))
                    {
                        cm = args[i + 1];
                    }
                }
            }

            return cm;
        }
    }
}

