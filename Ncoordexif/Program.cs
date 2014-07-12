using System;
using System.Drawing;
using System.Net.Mime;

namespace Ncoordexif
{
    class Program
    {
        static void Main(string[] args)
        {
            StickInTheExif(args[0]);
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



            Console.Write(gps.Latitude);
        }
    }
}
