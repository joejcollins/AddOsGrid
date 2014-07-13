using System;
using System.Drawing;
using DotNetCoords;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
            }
            else
            {
                try
                {
                    AddOsGrid(args[0]);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Don't understand.");
                }
            }
        }


        /// <summary>
        /// Print a suitable usage message, a suitable command line might be:
        /// </summary>
        private static void PrintUsage()
        {
            Console.WriteLine();
            Console.WriteLine("AddOSGrid imagefile.jpg");
            Console.WriteLine();
            Console.WriteLine("Renames file to OSXXXXXX_imagefile.jpg and changes EXIF description.");
        }

        private static void AddOsGrid(String filename)
        {
            Image image = Image.FromFile(filename);
            GpsMetaData gpsMetaData = image.GetGpsInfo();
            var latLng = new LatLng(gpsMetaData.Latitude, gpsMetaData.Longitude);
            var osRef = new OSRef(latLng);
            string sixFigureOsRef = osRef.ToSixFigureString();
            image.SetDescription(sixFigureOsRef);
            image.Save(sixFigureOsRef + "_" + filename);
        }
    }
}
