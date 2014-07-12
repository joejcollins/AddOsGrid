using System;
using System.Drawing;
using DotNetCoords;

namespace AddOSGrid
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                StickInTheExif(options.ImageFile);   
            }
            Console.ReadLine();
        }


        /// <summary>
        /// Print a suitable usage message, a suitable command line might be:
        /// </summary>
        private static void PrintUsage()
        {
            Console.WriteLine();
            Console.WriteLine("Blah");
        }

        private static void StickInTheExif(String filename)
        {
            Image i = Image.FromFile(filename);
            GpsMetaData gps = i.GetGpsInfo();
            var latLng = new LatLng(gps.Latitude, gps.Longitude);
            var osRef = new OSRef(latLng);

            Console.Write(osRef.ToSixFigureString());
            i.SetDescription(osRef.ToSixFigureString());
            i.Save("OS_" + filename);
        }
    }
}
