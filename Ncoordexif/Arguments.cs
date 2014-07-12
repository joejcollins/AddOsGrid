
using System;
using Ncoordexif.CommandLine;

namespace Ncoordexif
{
    /// <summary>
    /// The command line can take two arguments.  The target version
    /// and whether or not a warning is shown.
    /// </summary>
    internal class Arguments
    {
        [Argument(ArgumentType.AtMostOnce, DefaultValue = 0, HelpText = "File name.")]
        public String Filename = "";
    }
}
